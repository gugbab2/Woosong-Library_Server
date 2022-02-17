# Woosong Library_Server
## 서버 소개
- C# 언어를 사용한 __WCF__ 서버를 사용하였습니다.
- 아래에 있는 쿼리문을 실행하여 DB를 셋팅한 뒤 서버를 실행하면 됩니다. 

## DB 소개
### 전체 CREATE문
__우송대 관계자__
- CREATE TABLE WSUPeople
    (
W_ID int CONSTRAINT PK_WSUPeople PRIMARY KEY NOT NULL, 
password VARCHAR(20) NOT NULL,
type VARCHAR(20) DEFAULT 'student' NOT NULL,
name VARCHAR(20) NOT NULL, 
department VARCHAR(50) NOT NULL
     ); 

__메타버스__
- CREATE TABLE WSUforest
    (
W_ID int CONSTRAINT FK_WSUforest FOREIGN KEY(W_ID) REFERENCES WSUPeople(W_ID) PRIMARY KEY NOT NULL,
name VARCHAR(20) NOT NULL,
character int DEFAULT 0 NOT NULL,
login int DEFAULT 0 NOT NULL,
access int DEFAULT 0 NOT NULL
    ); 
__책 정보(실물, EBOOK)__
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
__학생 대출__
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
__학생 위시리스트__
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

#### 우송대 도서관에 있는 책 리스트(BookList) - <실물+전자>
