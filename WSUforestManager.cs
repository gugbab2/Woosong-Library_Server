using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WCF_RESTful
{
    //오류를 throw로 보내고 catch는 호출하는 쪽에서...
    public class WSUforestManager
    {
        //const string connstring = @"Server=DESKTOP-0I86BTV;database=WB34;uid=yhy;pwd=yhy";
        //const string connstring = @"Server=SOEUN-LAPTOP;database=WB34;uid=cse;pwd=cse";
        const string connstring = @"Server=DESKTOP-NTTAC6K\SQLEXPRESS;database=WB34;uid=nayoun;pwd=nayoun";
        private SqlConnection con = new SqlConnection();

        #region 데이터베이스 

        private void DB_Open()
        {
            con.ConnectionString = connstring;
            con.Open();
        }

        private void DB_Close()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        private void ExcuteNonQuery(string cmdtext)
        {
            SqlCommand command = new SqlCommand(cmdtext, con);
            if (command.ExecuteNonQuery() <= 0)
                throw new Exception("오류");
        }

        #endregion

        #region 브라우저 --> 서비스 

        // 로그인 유무 확인하기
        public string LoginCheck(int W_id)
        {
            WSUforest wsu = GetWSUforest(W_id);

            //이미 로그인을 한 상태
            if (wsu.Login)
            {
                return "001,로그인상태";
            }
            return "002,로그아웃상태";
        }

        // 도서 찜 유무 확인
        public int BookHeartCheck(int W_id, int b_id)
        {
            DB_Open();

            // 찜 정보 확인
            string sql = string.Format("Select COUNT(*) from WSUlibrary_BookHeart where W_ID = {0} AND B_ID ={1};", W_id, b_id);
            SqlCommand cmd = new SqlCommand(sql, con);
            int count = Convert.ToInt32(cmd.ExecuteScalar());

            DB_Close();

            return count;
        }

        // 도서 대출 현황 확인
        public string BookRentalCheck(int W_id, int b_id)
        {
            // 빌리는 책 정보 리스트
            WSUlibrary_BookList bookList = GetWSUlibrary_BookList(b_id);
            // 우송인이 빌린 책 리스트
            WSUlibrary_BookRental bookRental = GetWSUlibrary_BookRental(W_id);

            // 책이 빌려진 상태일 때
            if (bookList.Status == "rental")
            {
                // 본인이 빌렸을 때
                if (bookRental.W_ID == W_id)
                {
                    return "011,도서본인이대출상태";
                }
                // 본인 말고 다른 사람이 빌려갔을 때
                else if (bookRental.W_ID == 0)
                {
                    return "012,도서다른사람이대출상태";
                }
            }
            // 책이 도서관에 있을 때
            else if(bookList.Status == "library")
            {
                return "013,도서반납상태";
            }
            // 책이 파손되었을 때
            else if (bookList.Status == "broken")
            {
                return "014,도서파손상태";
            }
            // 책이 예약되었을 때
            else if (bookList.Status == "reservation")
            {
                return "015,도서예약상태";
            }
            return "014,도서대출현황 확인 실패";
        }

        // 로그인 = 학생 정보 확인(인증) 
        public string Login(int W_id, string password)
        {
            DB_Open();

            // 우송인 정보 검색
            string sql = string.Format("SELECT * from WSUPeople WHERE W_ID = {0} AND password = '{1}';", W_id, password);
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();

            // 우송인 정보 저장
            WSUPeople ple = null;
            while (reader.Read())
            {
                ple = new WSUPeople(
                    (int)reader["W_ID"],
                    (string)reader["password"],
                    (string)reader["type"],
                    (string)reader["name"],
                    (string)reader["department"]
                    );
            }

            DB_Close();

            //우송인 정보가 없는 상태 =  우송대 학생이 아니다.
            if (ple == null)
            {
                return "101,로그인 실패,로그인 정보가 올바르지 않습니다.";
            }

            WSUforest wsu = GetWSUforest(W_id);
            // 처음 접속하는 상태
            if (wsu.W_ID == 0)
            {
                // 데이터를 생성한다.
                InsertWSUforest(W_id);

                // 데이터를 다시 받아온다.
                wsu = GetWSUforest(W_id);
            }

            //이미 로그인을 한 상태
            if (wsu.Login)
            {
                LoginoutCheck(W_id, 0);
                return "102,로그인 실패, 이미 로그인 되어있습니다. 계정에서 로그아웃합니다.";
            }

            //우송인 정보가 있고 로그인을 하지 않은 상태--> DB의 Login 값을 1로 변경
            LoginoutCheck(W_id, 1);

            return "100,로그인 성공";
        }

        // login의 값을 바꾸는 메서드 -> LoginoutCheck(int id, int '0 또는 1')
        public bool LoginoutCheck(int W_id, int check)
        {
            try
            {
                DB_Open();

                string sql = string.Format("UPDATE WSUforest SET login = {0} WHERE W_ID = {1};", check, W_id);

                SqlCommand command = new SqlCommand(sql, con);
                if (command.ExecuteNonQuery() <= 0)
                {
                    return false;
                }
                return true;
            }
            finally
            {
                DB_Close();
            }
        }

        // 도서 검색(상세)
        public WSUlibrary_BookList BookInfoSelect(int b_id)
        {
            DB_Open();

            string sql = string.Format("SELECT * FROM WSUlibrary_BookList WHERE b_id = {0};", b_id);
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();

            WSUlibrary_BookList Data = null;
            while (reader.Read())
            {
                Data = new WSUlibrary_BookList(
                    (int)reader["B_ID"],
                    (string)reader["type"],
                    (string)reader["title"],
                    (string)reader["contents"],
                    (string)reader["isbn"],
                    (DateTime)reader["publishingdate"],
                    (string)reader["authors"],
                    (string)reader["publisher"],
                    (string)reader["translators"],
                    (string)reader["thumbnail"],
                    (string)reader["status"],
                    (int)reader["bestseller"]
                    );
            }

            DB_Close();

            return Data;
        }

        // 도서 검색
        public List<string> BookSelect(string type, string data)
        {
            DB_Open();

            string sql = null;

            // 책제목
            if (type == "title")
            {
                sql = string.Format("SELECT B_ID,type,title,authors,thumbnail FROM WSUlibrary_BookList WHERE title like '%{0}%';", data);
            }
            if (type == "authors")
            {
                sql = string.Format("SELECT B_ID,type,title,authors,thumbnail FROM WSUlibrary_BookList WHERE authors like '%{0}%';", data);
            }
            if (type == "publisher")
            {
                sql = string.Format("SELECT B_ID,type,title,authors,thumbnail FROM WSUlibrary_BookList WHERE publisher like '%{0}%';", data);
            }
            if (type == "b_id")
            {
                sql = string.Format("SELECT B_ID,type,title,authors,thumbnail FROM WSUlibrary_BookList WHERE b_id = {0};", data);
            }
            if (type == "isbn")
            {
                sql = string.Format("SELECT B_ID,type,title,authors,thumbnail FROM WSUlibrary_BookList WHERE isbn = '{0}';", data);
            }

            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();

            List<string> Data = new List<string>();
            while (reader.Read())
            {
                Data.Add(
                    ((int)reader["B_ID"]).ToString() + "@" +
                    (string)reader["type"] + "@" +
                    (string)reader["title"] + "@" +
                    (string)reader["authors"] + "@" +
                    (string)reader["thumbnail"]
                    );
            }

            DB_Close();

            return Data;
        }

        // 도서 대출 목록
        public List<string> BookCheckOutList(int W_id)
        {
            DB_Open();

            string sql = string.Format("SELECT W_ID, B_ID, type, title, rentaldate, returndate, renewcount, overduedays FROM WSUlibrary_BookRental WHERE W_ID={0};", W_id);
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();

            List<string> Data = new List<string>();
            while (reader.Read())
            {
                Data.Add(
                    ((int)reader["W_ID"]).ToString() + "@" +
                    ((int)reader["B_ID"]).ToString() + "@" +
                    (string)reader["type"] + "@" +
                    (string)reader["title"] + "@" +
                    ((DateTime)reader["rentaldate"]).ToString("d") + "@" +
                    ((DateTime)reader["returndate"]).ToString("d") + "@" +
                    ((int)reader["renewcount"]).ToString() + "@" +
                    ((int)reader["overduedays"]).ToString()
                    );
            }

            DB_Close();

            return Data;
        }

        // 도서 찜 목록
        public List<string> BookHeartList(int W_id)
        {
            DB_Open();

            string sql = string.Format("SELECT B_ID, type, title, authors, thumbnail FROM WSUlibrary_BookHeart WHERE W_ID={0};", W_id);
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();

            List<string> Data = new List<string>();
            while (reader.Read())
            {
                Data.Add(
                    ((int)reader["B_ID"]).ToString() + "@" +
                    (string)reader["type"] + "@" +
                    (string)reader["title"] + "@" +
                    (string)reader["authors"] + "@" +
                    (string)reader["thumbnail"]
                    );
            }

            DB_Close();

            return Data;
        }

        // 도서 베스트셀러 목록
        public List<string> BookBestSellerList()
        {
            DB_Open();

            string sql = string.Format("SELECT B_ID, type, title, authors, thumbnail FROM WSUlibrary_BookList WHERE bestseller != 0 ORDER BY Bestseller;");
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();

            List<string> Data = new List<string>();
            while (reader.Read())
            {
                Data.Add(
                    ((int)reader["B_ID"]).ToString() + "@" +
                    (string)reader["type"] + "@" +
                    (string)reader["title"] + "@" +
                    (string)reader["authors"] + "@" +
                    (string)reader["thumbnail"]
                    );
            }

            DB_Close();

            return Data;
        }

        // 도서 대출
        public string BookCheckOut(int W_id, int b_id)
        {
            // 우송인 정보 리스트
            WSUPeople ple = GetWSUPeople(W_id);
            // 빌리는 책 정보 리스트
            WSUlibrary_BookList bookList = GetWSUlibrary_BookList(b_id);
            // 연체일
            int Overdue_days = CheckOverdue_days(W_id);
            DateTime now = DateTime.Now;

            // 도서 ID가 잘못되었을 때
            if (bookList.B_ID == 0)
            {
                return "131,도서대출 실패,도서 정보가 올바르지 않습니다.";
            }

            // 책의 상태 = library, rental, broken, reservation 
            // 빌리는 책의 상태가 library = 대출가능
            if (bookList.Status == "library")
            {

                // 책의 타입 = real, e
                // 빌리는 책이 타입이 전자책일 경우(대출 가능 권수: 10권, 기간: 10일)
                if (bookList.Type == "e")
                {
                    // 연체날이 남아있을 때 (0이 미연체, 1이상이 연체)
                    if (Overdue_days > 0)
                    {
                        // 현재날짜에 연체날을 더해서 에러로 보여줌
                        DateTime OverdueDate = now.AddDays(Overdue_days);
                        return "133,도서대출 실패," + OverdueDate.ToString("d") + "까지 대출/예약중지 기간입니다.";
                    }

                    // 대출 가능 권수 10권을 넘겼을 때
                    //--> 대출한 책을 검색해서 type가 e인 행이 10개 이상일 때
                    if (GetBookRentalCount(W_id, "e") >= 10)
                    {
                        return "134-1,도서대출 실패, Ebook는 최대 10권까지 대출 가능합니다.";
                    }

                    // 대출기간 10일
                    InsertBookRental(W_id, b_id, bookList.Type, bookList.Title, 10);
                }

                // 빌리는 책이 타입이 실물책일 경우
                else if (bookList.Type == "real")
                {

                    // 우송인 Type =  student, employee, assistant, gradstudent, professor 
                    // 우송인 Type이 student(대출 가능 권수: 5권, 기간: 10일)
                    if (ple.Type == "student")
                    {

                        // 연체날이 남아있을 때 (0이 미연체, 1이상이 연체)
                        if (Overdue_days > 0)
                        {
                            // 현재날짜에 연체날을 더해서 에러로 보여줌
                            DateTime OverdueDate = now.AddDays(Overdue_days);
                            return "133,도서대출 실패," + OverdueDate.ToString("d") + "까지 대출중지 기간입니다.";
                        }

                        // 대출 가능 권수 5권을 넘겼을 때
                        if (GetBookRentalCount(W_id, "real") >= 5)
                        {
                            return "134-2,도서대출 실패, 학생은 최대 5권까지 대출 가능합니다.";
                        }

                        // 대출기간 10일
                        InsertBookRental(W_id, b_id, bookList.Type, bookList.Title, 10);

                    }
                    // 우송인 Type이 employee, assistant, gradstudent(대출 가능 권수: 5권, 기간: 15일)
                    else if (ple.Type == "employee" || ple.Type == "assistant" || ple.Type == "gradstudent")
                    {

                        // 연체날이 남아있을 때 (0이 미연체, 1이상이 연체)
                        if (Overdue_days > 0)
                        {
                            // 현재날짜에 연체날을 더해서 에러로 보여줌
                            DateTime OverdueDate = now.AddDays(Overdue_days);
                            return "133,도서대출 실패," + OverdueDate.ToString("d") + "까지 대출중지 기간입니다.";
                        }

                        // 대출 가능 권수 5권을 넘겼을 때
                        if (GetBookRentalCount(W_id, "real") >= 5)
                        {
                            return "134-3,도서대출 실패, 직원/조교/대학원생은 최대 5권까지 대출 가능합니다.";
                        }

                        // 대출기간 15일
                        InsertBookRental(W_id, b_id, bookList.Type, bookList.Title, 15);
                    }
                    // 우송인 Type이 professor(대출 가능 권수: 10권, 기간: 30일)
                    else if (ple.Type == "professor")
                    {

                        // 연체날이 남아있을 때 (0이 미연체, 1이상이 연체)
                        if (Overdue_days > 0)
                        {
                            // 현재날짜에 연체날을 더해서 에러로 보여줌
                            DateTime OverdueDate = now.AddDays(Overdue_days);
                            return "133,도서대출 실패," + OverdueDate.ToString("d") + "까지 대출중지 기간입니다.";
                        }

                        // 대출 가능 권수 10권을 넘겼을 때
                        if (GetBookRentalCount(W_id, "real") >= 10)
                        {
                            return "134-4,도서대출 실패, 교수는 최대 10권까지 대출 가능합니다.";
                        }

                        // 대출기간 30일
                        InsertBookRental(W_id, b_id, bookList.Type, bookList.Title, 30);
                    }
                }
            }
            else if (bookList.Status == "rental")
            {
                return "132-1,도서대출 실패,이미 대출한 도서입니다.";
            }
            else if (bookList.Status == "broken")
            {
                return "132-2,도서대출 실패,파손된 도서입니다.";
            }
            else if (bookList.Status == "reservation")
            {
                return "132-3,도서대출 실패,예약된 도서입니다.";
            }

            // WSUlibrary_BookList의 status를 rental로 바꾸기
            UpdateBookListStatus(b_id, "rental");

            return "130,도서대출 성공";
        }

        // 도서 반납
        // 도서 반납 -> 반납일이 지났는지 확인 후 DB에서 삭제
        public string BookReturn(int W_id, int b_id)
        {
            // 연체일
            int Overdue_returndate = CheckOverdue_returndate(b_id);

            // 반납하려는 책이 현재 시점으로 연체중일 때
            if (Overdue_returndate > 0)
            {
                // 현재날짜에 연체날을 더해서 에러로 보여줌
                DateTime now = DateTime.Now;
                DateTime OverdueDate = now.AddDays(Overdue_returndate);

                // WSUlibrary_BookRental에서 type에 연체되었다고 표기해주기
                DB_Open();
                string sql0 = string.Format("UPDATE WSUlibrary_BookRental SET overduedays={0}, type='연체' where W_id={1} AND b_id={2};",
                                            Overdue_returndate, W_id, b_id);
                ExcuteNonQuery(sql0);
                DB_Close();

                // WSUlibrary_BookList의 status를 library로 바꾸기
                UpdateBookListStatus(b_id, "library");

                return "141,도서반납 성공," + OverdueDate.ToString("d") + "까지 대출/예약중지 입니다.";
            }

            // WSUlibrary_BookRental에서 삭제
            DB_Open();
            string sql = string.Format("DELETE FROM WSUlibrary_BookRental WHERE W_ID = {0} AND B_ID={1};",
                                        W_id, b_id);
            ExcuteNonQuery(sql);
            DB_Close();

            // WSUlibrary_BookList의 status를 library로 바꾸기
            UpdateBookListStatus(b_id, "library");

            return "140,도서반납 성공";
        }

        // 도서 대출 연장 
        public string BookRenew(int W_id, int b_id)
        {
            // 우송인 정보 리스트
            WSUPeople ple = GetWSUPeople(W_id);
            // 빌리는 책 정보 리스트
            WSUlibrary_BookList bookList = GetWSUlibrary_BookList(b_id);
            // 우송인이 빌린 책 리스트
            WSUlibrary_BookRental bookRental = GetWSUlibrary_BookRental(W_id);
            // 연체 유무
            int Overdue_days = CheckOverdue_days(W_id);
            // 연체일
            int Overdue_returndate = CheckOverdue_returndate(b_id);
            DateTime now = DateTime.Now;

            // 이미 대출 연장을 했을 때
            if (bookRental.Renewcount == 1)
            {
                return "152,도서대출연장 실패, 대출 연장은 최대 1회 가능합니다.";
            }

            // 연체날이 남아있을 때 (0이 미연체, 1이상이 연체)
            if (Overdue_days > 0)
            {
                // 현재날짜에 연체날을 더해서 에러로 보여줌
                DateTime OverdueDate = now.AddDays(Overdue_days);
                return "153,도서대출연장 실패," + OverdueDate.ToString("d") + "까지 대출/예약중지 기간입니다.";
            }

            // 연장하려는 책이 현재 시점으로 연체중일 때
            if (Overdue_returndate > 0)
            {
                return "154,도서대출연장 실패,현재" + Overdue_returndate.ToString() + "일 연체된 도서입니다. 도서를 반납해주세요.";
            }

            // 사람 type에 따른 도서 연장
            // 10일
            if (bookList.Type == "e" || ple.Type == "student")
            {
                UpdateBookreturndate(W_id, b_id, 10);
            }

            // 15일
            else if (ple.Type == "employee" || ple.Type == "assistant" || ple.Type == "gradstudent")
            {
                UpdateBookreturndate(W_id, b_id, 15);
            }

            // 30일
            else if (ple.Type == "professor")
            {
                UpdateBookreturndate(W_id, b_id, 30);
            }

            return "150,도서대출연장 성공";
        }

        // 도서 찜 선택
        public string BookHeart(int W_id, int b_id)
        {
            if (BookHeartCheck(W_id, b_id) == 1)
            {
                return "171,도서찜 실패 : 이미 찜한 도서입니다.";
            }

            // 우송대 책 리스트
            WSUlibrary_BookList bookList = GetWSUlibrary_BookList(b_id);
            DB_Open();
            string sql = string.Format("INSERT INTO WSUlibrary_BookHeart(W_ID, B_ID, title, authors, thumbnail)VALUES({0},{1},'{2}','{3}','{4}');",
                                        W_id, b_id, bookList.Title, bookList.Authors, bookList.Thumbnail);
            ExcuteNonQuery(sql);
            DB_Close();

            return "170,도서찜 성공";
        }

        // 도서 찜 해제
        public string BookUnHeart(int W_id, int b_id)
        {
            DB_Open();
            string sql = string.Format("DELETE FROM WSUlibrary_BookHeart WHERE W_ID={0} AND B_ID={1};", W_id, b_id);
            ExcuteNonQuery(sql);
            DB_Close();

            return "180,도서찜 해제 성공";
        }

        #endregion

        #region 유니티 --> 서비스

        // 게임 접속(접속상태 체크 후 access값 변경)
        public bool GameJoin(int W_id)
        {
            WSUforest wsu = GetWSUforest(W_id);

            // 이미 게임 접속을 한 상태
            if (wsu.Access == true)
            {
                throw new Exception("이미 접속 되어있습니다.");
            }

            // 게임접속--> DB의 Access 값을 1로 변경
            GameJoinCheck(wsu.W_ID, 1);
            return true;
        }

        // access의 값을 바꾸는 메서드 ->  GameJoinCheck(int id, int '0 또는 1')
        public bool GameJoinCheck(int W_id, int check)
        {
            try
            {
                DB_Open();

                string sql = string.Format("UPDATE WSUforest SET access = {0} WHERE W_ID = {1};",
                                            check, W_id);

                SqlCommand command = new SqlCommand(sql, con);
                if (command.ExecuteNonQuery() <= 0)
                {
                    return false;
                }
                return true;
            }
            finally
            {
                DB_Close();
            }
        }

        // Student_WSUforest정보 받아와서 W_ID, Name,Character를 string로 넘기기
        public string GetPlayerdata(int W_id)
        {
            WSUforest wsu = GetWSUforest(W_id);

            return wsu.W_ID.ToString() + '@' + wsu.Name + '@' + wsu.Character.ToString();
        }

        // WSUforest character 수정
        public bool UpdateCustom(int W_id, int custom)
        {
            DB_Open();
            string sql = string.Format("UPDATE WSUforest SET character = {0} WHERE W_ID = {1};",
                                        custom, W_id);
            ExcuteNonQuery(sql);
            DB_Close();
            return true;
        }

        // 도서 검색(유니티에서 사용)
        public string GetBookdata(string title, string type)
        {
            DB_Open();

            string sql = string.Format("SELECT * FROM WSUlibrary_BookList WHERE title = '{0}' AND type = '{1}';", title, type);
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();

            string Data = null;
            while (reader.Read())
            {
                Data =
                        ((int)reader["B_ID"]).ToString() + '@' +
                        (string)reader["type"] + '@' +
                        (string)reader["title"] + '@' +
                        (string)reader["contents"] + '@' +
                        (string)reader["isbn"] + '@' +
                        (string)reader["authors"] + '@' +
                        (string)reader["publisher"] + '@' +
                        (string)reader["translators"] + '@' +
                        (string)reader["thumbnail"] + '@' +
                        (string)reader["status"] + '@' +
                        ((int)reader["bestseller"]).ToString()
                        ;
            }

            DB_Close();

            if (Data == null)
            {
                throw new Exception();
            }

            return Data;
        }

        // 베스트셀러 가져오기(유니티에서 사용)
        public List<string> Unity_BestSelect()
        {
            DB_Open();

            string sql = string.Format("SELECT title,thumbnail,bestseller FROM WSUlibrary_BookList WHERE bestseller BETWEEN 1 AND 6 ORDER BY bestseller;");
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();

            List<string> Data = new List<string>();
            while (reader.Read())
            {
                Data.Add((string)reader["title"] + '@' +
                         (string)reader["thumbnail"] + '@' +
                         ((int)reader["bestseller"]).ToString());
            }

            DB_Close();

            return Data;
        }

        #endregion

        #region 내부 함수들...(StudentManager에서만 사용)

        // WSUPeople(우송인)의 정보를 받아오는 메서드 - W_ID
        private WSUPeople GetWSUPeople(int W_id)
        {
            DB_Open();

            //우송인 정보 검색
            string sql = string.Format("SELECT * from WSUPeople WHERE W_ID = {0};", W_id);
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();

            //우송인 정보 저장
            WSUPeople ple = null;
            while (reader.Read())
            {
                ple = new WSUPeople(
                    (int)reader["W_ID"],
                    (string)reader["password"],
                    (string)reader["type"],
                    (string)reader["name"],
                    (string)reader["department"]
                    );
            }

            DB_Close();

            return ple;
        }

        // WSUforest(유저)의 정보를 받아오는 메서드 - W_ID
        private WSUforest GetWSUforest(int W_id)
        {
            DB_Open();

            //유저 정보 검색
            string sql = string.Format("SELECT * from WSUforest WHERE W_ID = {0};", W_id);
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();

            //유저 정보 저장
            WSUforest wsu = null;
            while (reader.Read())
            {
                wsu = new WSUforest(
                    (int)reader["W_ID"],
                    (string)reader["name"],
                    (int)reader["character"],
                    Convert.ToBoolean((int)reader["login"]),
                    Convert.ToBoolean((int)reader["access"])
                    );
            }

            DB_Close();

            if (wsu == null)
            {
                wsu = new WSUforest(0, null, 0, false, false);
            }

            return wsu;
        }

        // WSUlibrary_BookList(책)의 정보를 받아오는 메서드 - B_ID
        private WSUlibrary_BookList GetWSUlibrary_BookList(int b_id)
        {
            DB_Open();

            // 책 정보 검색
            string sql = string.Format("SELECT * FROM WSUlibrary_BookList WHERE B_ID = {0};", b_id);
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();

            // 책 정보 저장
            WSUlibrary_BookList bookList = null;
            while (reader.Read())
            {
                bookList = new WSUlibrary_BookList(
                    (int)reader["B_ID"],
                    (string)reader["type"],
                    (string)reader["title"],
                    (string)reader["contents"],
                    (string)reader["isbn"],
                    (DateTime)reader["publishingdate"],
                    (string)reader["authors"],
                    (string)reader["publisher"],
                    (string)reader["translators"],
                    (string)reader["thumbnail"],
                    (string)reader["status"],
                    (int)reader["bestseller"]
                    );
            }

            DB_Close();

            if (bookList == null)
            {
                bookList = new WSUlibrary_BookList(0, null, null, null, null, DateTime.Now, null, null, null, null, null, 0);
            }

            return bookList;
        }

        // WSUlibrary_BookRental의 정보를 받아오는 메서드 - W_ID
        private WSUlibrary_BookRental GetWSUlibrary_BookRental(int W_id)
        {
            DB_Open();

            string sql = string.Format("SELECT W_ID, B_ID, type, title, rentaldate, returndate, renewcount, overduedays FROM WSUlibrary_BookRental WHERE W_ID={0};", W_id);
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader = cmd.ExecuteReader();

            WSUlibrary_BookRental bookRental = null;
            while (reader.Read())
            {
                bookRental = new WSUlibrary_BookRental(
                    (int)reader["W_ID"],
                    (int)reader["B_ID"],
                    (string)reader["type"],
                    (string)reader["title"],
                    (DateTime)reader["rentaldate"],
                    (DateTime)reader["returndate"],
                    (int)reader["renewcount"],
                    (int)reader["overduedays"]
                    );
            }

            DB_Close();

            if (bookRental == null)
            {
                bookRental = new WSUlibrary_BookRental(0, 0, null, null, DateTime.Now, DateTime.Now, 0, 0);
            }

            return bookRental;
        }

        // WSUlibrary_BookRental 생성 - 도서 대출
        private void InsertBookRental(int W_id, int b_id, string type, string title, int days)
        {
            DB_Open();
            string sql = string.Format("INSERT INTO WSUlibrary_BookRental(W_ID,B_ID, type, title, returndate)VALUES({0}, {1}, '{2}', '{3}', DATEADD(DAY, {4}, GETDATE()));",
                                        W_id, b_id, type, title, days);
            ExcuteNonQuery(sql);
            DB_Close();
        }

        // 연체 유무와 연체일 확인(이미 반납한 책을 검사 + overduedays > 0 이면 연체)
        private int CheckOverdue_days(int W_id)
        {
            try
            {
                DB_Open();

                string sql = string.Format("SELECT MAX(overduedays) FROM WSUlibrary_BookRental WHERE W_id={0} AND overduedays != 0;", W_id);
                SqlCommand cmd = new SqlCommand(sql, con);

                if (cmd.ExecuteScalar() == DBNull.Value)
                {
                    return 0;
                }
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count;
            }
            finally
            {
                DB_Close();
            }
        }

        // 연체 유무 확인(현재 반납하는 책을 검사 + returndate - rentaldate > 0 이면 연체)
        private int CheckOverdue_returndate(int b_id)
        {
            try
            {
                DB_Open();

                string sql = string.Format("SELECT DATEDIFF(day, returndate, GETDATE()) FROM WSUlibrary_BookRental WHERE B_ID={0};", b_id);
                SqlCommand cmd = new SqlCommand(sql, con);

                if (cmd.ExecuteScalar() == DBNull.Value)
                {
                    return 0;
                }
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count;
            }
            finally
            {
                DB_Close();
            }
        }

        // WSUforest 생성 -  처음 로그인
        private void InsertWSUforest(int W_id)
        {
            WSUPeople ple = GetWSUPeople(W_id);

            DB_Open();
            string sql = string.Format("INSERT INTO WSUforest(W_ID, name)VALUES({0},'{1}');",
                                        W_id, ple.Name);
            ExcuteNonQuery(sql);
            DB_Close();
        }

        // BookRental 대출한 권 수 
        private int GetBookRentalCount(int W_id, string type)
        {
            try
            {
                DB_Open();

                string sql = string.Format("SELECT COUNT(*) FROM WSUlibrary_BookRental WHERE W_ID={0} AND type='{1}';", W_id, type);
                SqlCommand cmd = new SqlCommand(sql, con);
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count;
            }
            finally
            {
                DB_Close();
            }
        }

        // BookList의 도서 상태(status)를 바꾸기
        private void UpdateBookListStatus(int b_id, string status)
        {
            DB_Open();
            string sql = string.Format("UPDATE WSUlibrary_BookList SET status='{0}' WHERE B_ID={1};",
                                        status, b_id);
            ExcuteNonQuery(sql);
            DB_Close();
        }

        // WSUlibrary_BookRental의 대출 연장(returndate)
        private void UpdateBookreturndate(int W_id, int b_id, int days)
        {
            DB_Open();
            string sql = string.Format("UPDATE WSUlibrary_BookRental SET returndate=DATEADD(DAY, {0}, returndate) WHERE W_ID={1} AND B_ID={2};",
                                        days, W_id, b_id);
            ExcuteNonQuery(sql);
            DB_Close();
        }

        #endregion

        // WSUlibrary_BookRental에서 type != '연체' (real or e)
        // 정각이 되면 연체(반납X)을 계산하여 자동으로 연체일을 +1하는 메서드

        // WSUlibrary_BookRental에서 type == '연체'
        // 정각이 되면 연체(반납o)한 책들을 계산하여 자동으로 연체일을 -1하는 메서드
        // overduedays가 0이 되면 WSUlibrary_BookRental에서 삭제

    }
}