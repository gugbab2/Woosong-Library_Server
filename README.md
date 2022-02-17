# Woosong Library_Server
## 서버 소개
- C# 언어를 사용한 __WCF__ 서버를 사용하였습니다.
- 아래에 있는 쿼리문을 실행하여 DB를 셋팅한 뒤 서버를 실행하면 됩니다. 

## DB 소개
### 전체 CREATE문
#### 우송대 관계자
- CREATE TABLE WSUPeople
    (
W_ID int CONSTRAINT PK_WSUPeople PRIMARY KEY NOT NULL, 
password VARCHAR(20) NOT NULL,
type VARCHAR(20) DEFAULT 'student' NOT NULL,
name VARCHAR(20) NOT NULL, 
department VARCHAR(50) NOT NULL
     ); 

#### 우송대 도서관 메타버스 이용자
- CREATE TABLE WSUforest
    (
W_ID int CONSTRAINT FK_WSUforest FOREIGN KEY(W_ID) REFERENCES WSUPeople(W_ID) PRIMARY KEY NOT NULL,
name VARCHAR(20) NOT NULL,
character int DEFAULT 0 NOT NULL,
login int DEFAULT 0 NOT NULL,
access int DEFAULT 0 NOT NULL
    ); 
    
#### 책 정보(실물, EBOOK)
- CREATE TABLE WSUlibrary_BookList
    (
B_ID int IDENTITY(1,1) CONSTRAINT PK_BookList  PRIMARY KEY NOT NULL,
type VARCHAR(20) NOT NULL,  
title VARCHAR(50) NOT NULL,
contents VARCHAR(100) NOT NULL, 
isbn VARCHAR(50) NOT NULL,
publishingdate date NOT NULL,
authors VARCHAR(100) NOT NULL,
publisher VARCHAR(100) NOT NULL,
translators VARCHAR(100)NOT NULL,
thumbnail VARCHAR(MAX)NOT NULL,
status VARCHAR(20) DEFAULT 'library' NOT NULL,
bestseller int DEFAULT 0 NOT NULL
     ); 
     
#### 학생 대출
- CREATE TABLE WSUlibrary_BookRental 
    (
rentalcount int IDENTITY(1,1) PRIMARY KEY NOT NULL,
W_ID int CONSTRAINT FK_BookRental_W_ID FOREIGN KEY(W_ID) REFERENCES WSUPeople(W_ID) NOT NULL,
B_ID int CONSTRAINT FK_BookRental_B_ID FOREIGN KEY(B_ID) REFERENCES WSUlibrary_BookList(B_ID) UNIQUE NOT NULL,
type VARCHAR(20) NOT NULL,
title VARCHAR(50) NOT NULL,
rentaldate date NOT NULL DEFAULT GETDATE(),
returndate date NOT NULL DEFAULT DATEADD(DAY, 10, GETDATE()),
renewcount int NOT NULL DEFAULT 0,
overduedays int NOT NULL DEFAULT 0
);

#### 학생 위시리스트
- CREATE TABLE WSUlibrary_BookHeart
    (
heartcount int IDENTITY(1,1) PRIMARY KEY NOT NULL,
W_ID int CONSTRAINT FK_BookHeart_W_ID FOREIGN KEY(W_ID) REFERENCES WSUPeople(W_ID) NOT NULL,
B_ID int CONSTRAINT FK_BookHeart_B_ID FOREIGN KEY(B_ID) REFERENCES WSUlibrary_BookList(B_ID) UNIQUE NOT NULL,
type VARCHAR(20) NOT NULL,
title VARCHAR(50) NOT NULL,
authors VARCHAR(50) NOT NULL,
thumbnail VARCHAR(MAX) NOT NULL
    );
### INSERT문
#### 우송대 관계자 리스트(WSUPeople) 
- INSERT INTO WSUPeople(W_ID, password, name, department)VALUES(11111111, 'A1', '추윤성', '사회복지학과'); 
INSERT INTO WSUPeople(W_ID, password, Name, department)VALUES(11112222, 'B2', '복소연', '글로벌아동교육학과'); 
INSERT INTO WSUPeople(W_ID, password, Name, department)VALUES(11113333, 'C3', '황보은성', '작업치료학과'); 
INSERT INTO WSUPeople(W_ID, password, Name, department)VALUES(11114444, 'D4', '백정우', '언어치료청각재활학과'); 
INSERT INTO WSUPeople(W_ID, password, Name, department)VALUES(11115555, 'E5', '풍성기', '보건의료경영학과'); 
INSERT INTO WSUPeople(W_ID, password, Name, department)VALUES(11116666, 'F6', '한명희', '유아교육과'); 
INSERT INTO WSUPeople(W_ID, password, Name, department)VALUES(11117777, 'G7', '문서우', '뷰티디자인경영학과'); 
INSERT INTO WSUPeople(W_ID, password, Name, department)VALUES(11118888, 'H8', '황성호', '응급구조학과'); 
INSERT INTO WSUPeople(W_ID, password, Name, department)VALUES(11119999, 'I9', '홍선웅', '소방안전학부(소방방재전공,안전공학전공)'); 
INSERT INTO WSUPeople(W_ID, password, Name, department)VALUES(22220000, 'J10', '신홍식', '간호학과'); 
INSERT INTO WSUPeople(W_ID, password, Name, department)VALUES(22221111, 'K11', '오철진', '물리치료학과'); 
INSERT INTO WSUPeople(W_ID, password, Name, department)VALUES(22222222, 'L12', '안경주', '기획팀'); 
INSERT INTO WSUPeople(W_ID, type, password, Name, department)VALUES(22223333, 'employee', 'M13', '풍재아', '시설팀'); 
INSERT INTO WSUPeople(W_ID, type, password, Name, department)VALUES(22224444, 'employee', 'N14', '사공남준', '외식조리학부(글로벌한식조리전공)');
INSERT INTO WSUPeople(W_ID, type, password, Name, department)VALUES(22225555, 'assistant', 'O15', '류도현', '외식조리학부(외식,조리경영전공)'); 
INSERT INTO WSUPeople(W_ID, type, password, Name, department)VALUES(22226666, 'assistant', 'P16', '풍서준', '외식조리영양학부'); 
INSERT INTO WSUPeople(W_ID, type, password, Name, department)VALUES(22227777, 'gradstudent', 'Q17', '권선호', '호텔관광경영학과'); 
INSERT INTO WSUPeople(W_ID, type, password, Name, department)VALUES(22228888, 'gradstudent', 'R18', '조성철', 'IT융합학부(컴퓨터정보보안전공)'); 
INSERT INTO WSUPeople(W_ID, type, password, Name, department)VALUES(22229999, 'professor', 'S19', '탁승우', 'IT융합학부(스마트IT보안전공)'); 
INSERT INTO WSUPeople(W_ID, type, password, Name, department)VALUES(33330000, 'professor', 'T20', '장은남', '테크노미디어융합학부(게임멀티미디어전공)');

#### 우송대 도서관에 있는 책 리스트(BookList) - <실물+전자>
- INSERT INTO WSUlibrary_BookList(type, title, contents, isbn, publishingdate, authors, publisher, translators, thumbnail, Status, Bestseller)
VALUES('real',
'C언어로쉽게풀어쓴자료구조', '개정판은초판과마찬가지로학습자들이좀더쉽게자료구조를이해할수있도록하자는것을목표',
'9788970509716','20190222','천인국,공용해,하상호','생능출판','없음', 
'https://library.wsu.ac.kr/Sponge/Images/bookDefaults/MMbookdefaultsmall.png', 'library', 8 )
INSERT INTO WSUlibrary_BookList(type, title, contents, isbn, publishingdate, authors, publisher, translators, thumbnail, Status, Bestseller)
VALUES('e',
'윤성우열혈C++ 프로그래밍', '2004년도에출간된윤성우저자「열혈강의C++ 프로그래밍」의개정판',
'9788996094043', '20100512', '윤성우', '오렌지미디어', '없음', 
'https://library.wsu.ac.kr/Sponge/Images/bookDefaults/CCbookdefaultsmall.png', 'library', 2 )
INSERT INTO WSUlibrary_BookList(type, title, contents, isbn, publishingdate, authors, publisher, translators, thumbnail, Status, Bestseller)
VALUES('real',
'열혈강의자료구조', '자료구조의기본개념을쉽게이해할수있도록돕는책',
'9788989345022', '20100115', '이상진', '프리렉', '없음', 
'https://library.wsu.ac.kr/Sponge/Images/bookDefaults/MMbookdefaultsmall.png', 'library', 4 )
INSERT INTO WSUlibrary_BookList(type, title, contents, isbn, publishingdate, authors, publisher, translators, thumbnail, Status, Bestseller)
VALUES('e',
'혼자공부하는자바', '혼자해도충분하다! 1:1 과외하듯배우는자바프로그래밍자습서(JAVA 8 &11 지원)',
'9791162241875', '20190610', '신용권', '한빛미디어', '없음', 
'https://library.wsu.ac.kr/Sponge/Images/bookDefaults/CCbookdefaultsmall.png', 'library', 5 )
INSERT INTO WSUlibrary_BookList(type, title, contents, isbn, publishingdate, authors, publisher, translators, thumbnail,Status, Bestseller)
VALUES('e',
'조엘온소프트웨어', '엘온소프트웨어블러그에실렸던기사중에서유쾌하고핵심을찌르는베스트기사만뽑아내엮은책',
'9788989975588', '20050407', '조엘스폴스키', '에이콘출판', '박재호', 
'https://library.wsu.ac.kr/Sponge/Images/bookDefaults/CCbookdefaultsmall.png', 'library', 1 )
INSERT INTO WSUlibrary_BookList(type, title, contents, isbn, publishingdate, authors, publisher, translators, thumbnail, Status, Bestseller)
VALUES('real',
'Do it! C언어입문', '실무년강의년, 현업프로그래머가원리부터알려주는C언어!',
'9791187370703', '20170110', '김성엽', '이지스퍼블리싱', '없음', 
'https://library.wsu.ac.kr/Sponge/Images/bookDefaults/MMbookdefaultsmall.png', 'library', 3 )
INSERT INTO WSUlibrary_BookList(type, title, contents, isbn, publishingdate, authors, publisher, translators, thumbnail, Status, Bestseller)
VALUES('e',
'Do it! 점프투파이썬', '파이썬년연속베스트셀러위!《DO IT! 점프투파이썬》전면개정판출시!',
'9791163030911', '20190620', '박응용', '이지스퍼블리싱', '없음', 
'https://library.wsu.ac.kr/Sponge/Images/bookDefaults/CCbookdefaultsmall.png', 'library', 7 )
INSERT INTO WSUlibrary_BookList(type, title, contents, isbn, publishingdate, authors, publisher, translators, thumbnail, Status, Bestseller)
VALUES('real',
'대체뭐가문제야', '‘문제’인문제를제대로발견하는방법!',
'9788966260669', '20130125', '제럴드M. 와인버그,도널드고즈', '인사이트', '김준식', 
'https://library.wsu.ac.kr/Sponge/Images/bookDefaults/MMbookdefaultsmall.png', 'library', 6 )
INSERT INTO WSUlibrary_BookList(type, title, contents, isbn, publishingdate, authors, publisher, translators, thumbnail, Status, Bestseller)
VALUES('real',
'이것이C++이다', '저자가년간실무와강의를통해쌓은노하우를바탕으로C++를제대로입문할수있도록안내하는책',
'9788968482465', '20160201', '최호성', '한빛미디어', '없음', 
'https://library.wsu.ac.kr/Sponge/Images/bookDefaults/MMbookdefaultsmall.png', 'broken', 0 )
INSERT INTO WSUlibrary_BookList(type, title, contents, isbn, publishingdate, authors, publisher, translators, thumbnail, Status, Bestseller)
VALUES('e',
'Java의정석', '자바의기초부터실전활용까지모두담다!',
'9788994492032', '20160127', '남궁성', '도우출판','없음', 
'https://library.wsu.ac.kr/Sponge/Images/bookDefaults/CCbookdefaultsmall.png', 'reservation ', 0 )
