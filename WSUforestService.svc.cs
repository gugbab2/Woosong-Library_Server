using System;
using System.Collections.Generic;

namespace WCF_RESTful
{
    //catch는 오류를 받아서 클라이언트에 보여주기...

    public class WSUforestService : IWSUforestService
    {
        WSUforestManager WSUManager = new WSUforestManager();

        #region 브라우저 --> 서비스 

        // 로그인 유무 확인하기
        public string WSU_LoginCheck(string W_id)
        {
            try
            {
                return WSUManager.LoginCheck(int.Parse(W_id));
            }
            catch
            {
                return "003,로그인 체크 실패";
            }
        }

        // 도서 찜 유무 확인
        public string WSU_BookHeartCheck(string W_id, string b_id)
        {
            try
            {
                int count = WSUManager.BookHeartCheck(int.Parse(W_id), int.Parse(b_id));

                if (count == 0)
                {
                    return "022,찜해제상태";
                }
                return "021,찜상태";
            }
            catch
            {
                return "023,도서 찜 유무 확인 실패";
            }
        }

        // 도서 대출 현황 확인
        public string WSU_BookRentalCheck(string W_id, string b_id)
        {
            try
            {
                return WSUManager.BookRentalCheck(int.Parse(W_id), int.Parse(b_id));
            }
            catch
            {
                return "014,도서대출현황 확인 실패";
            }
        }

        // 로그인 요청
        public string WSU_Login(string W_id, string password)
        {
            return WSUManager.Login(int.Parse(W_id), password);
        }

        // 로그아웃 요청
        public string WSU_Logout(string W_id)
        {
            try
            {
                if (WSUManager.LoginoutCheck(int.Parse(W_id), 0))
                    return "110,로그아웃 성공";
                else
                    throw new Exception();
            }
            catch
            {
                return "111,로그아웃 실패";
            }
        }

        // 도서 검색(상세)
        public WSUlibrary_BookList WSU_BookInfoSelect(string b_id)
        {
            try
            {
                return WSUManager.BookInfoSelect(int.Parse(b_id));
            }
            catch
            {
                return null;
            }
        }

        // 도서 검색
        public List<string> WSU_BookSelect(string type, string data)
        {
            try
            {
                return WSUManager.BookSelect(type, data);
            }
            catch
            {
                return null;
            }
        }

        // 사람 대출 목록
        public List<string> WSU_BookCheckOutList(string W_id)
        {
            try
            {
                return WSUManager.BookCheckOutList(int.Parse(W_id));
            }
            catch
            {
                return null;
            }
        }

        // 사람 찜 목록
        public List<string> WSU_BookHeartList(string W_id)
        {
            try
            {
                return WSUManager.BookHeartList(int.Parse(W_id));
            }
            catch
            {
                return null;
            }
        }

        // 도서 베스트셀러 목록
        public List<string> WSU_BookBestSellerList()
        {
            try
            {
                return WSUManager.BookBestSellerList();
            }
            catch
            {
                return null;
            }
        }

        // 도서 대출
        public string WSU_BookCheckOut(string W_id, string b_id)
        {
            try
            {
                return WSUManager.BookCheckOut(int.Parse(W_id), int.Parse(b_id));
            }
            catch
            {
                return null;
            }
        }

        // 도서 반납
        public string WSU_BookReturn(string W_id, string b_id)
        {
            try
            {
                return WSUManager.BookReturn(int.Parse(W_id), int.Parse(b_id));
            }
            catch
            {
                return "142,도서반납 실패";
            }
        }

        // 도서 대출 연장
        public string WSU_BookRenew(string W_id, string b_id)
        {
            try
            {
                return WSUManager.BookRenew(int.Parse(W_id), int.Parse(b_id));
            }
            catch
            {
                return "151,도서대출연장 실패";
            }
        }

        // 도서 찜 선택
        public string WSU_BookHeart(string W_id, string b_id)
        {
            try
            {
                return WSUManager.BookHeart(int.Parse(W_id), int.Parse(b_id));
            }
            catch
            {
                return "171,도서찜 실패";
            }
        }

        // 도서 찜 해제
        public string WSU_BookUnHeart(string W_id, string b_id)
        {
            try
            {
                return WSUManager.BookUnHeart(int.Parse(W_id), int.Parse(b_id));
            }
            catch
            {
                return "181,도서찜 해제 실패";
            }
        }



        #endregion

        #region 유니티 --> 서비스

        // 게임 접속 -> 유니티가 처음 실행될 때 호출
        public string Unity_GameJoin(int id)
        {
            try
            {
                if (WSUManager.GameJoin(id))
                    return WSUManager.GetPlayerdata(id);
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {
                return "[게임접속 실패] " + ex.Message;
            }
        }

        // 게임 접속 종료 -> 유니티가 죽을 때 호출
        public string Unity_GameUnjoin(int id)
        {
            try
            {
                if (WSUManager.GameJoinCheck(id, 0))
                    return "[게임종료 성공]";
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {
                return "[게임종료 실패] " + ex.Message;
            }
        }

        // 플레이어 커스텀 저장 및 수정 
        public string Unity_UpdateCustom(int id, int custom)
        {
            try
            {
                if (WSUManager.UpdateCustom(id, custom))
                    return "[캐릭터 저장 성공]";
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {
                return "[캐릭터 저장 실패] " + ex.Message;
            }
        }

        // 도서 검색
        public string Unity_BookSelect(string title, string type)
        {
            try
            {
                return WSUManager.GetBookdata(title, type);
            }
            catch
            {
                return null;
            }
        }

        // 베스트셀러 검색
        public List<string> Unity_Bestseller()
        {
            try
            {
                return WSUManager.Unity_BestSelect();
            }
            catch
            {
                return null;
            }
        }

        // 찜
        public string Unity_BookHeart(int W_id, int b_id)
        {
            try
            {
                return WSUManager.BookHeart(W_id, b_id);
            }
            catch
            {
                return "171,도서찜 실패";
            }
        }

        // 찜 해제
        public string Unity_BookUnHeart(int W_id, int b_id)
        {
            try
            {
                return WSUManager.BookUnHeart(W_id, b_id);
            }
            catch
            {
                return "181,도서찜 해제 실패";
            }
        }

        #endregion
    }
}