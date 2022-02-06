using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

//--------------------------------------------------------------------------------

//WCF를 사용하기 위해서는 어셈블리
//1) 어셈블리명: System.ServiceModel;
//2) 네임스페이스명: using System.ServiceModel;

//---- 서비스 구성 절차

//1) interface를 만들면서 계약 구성
//   [ServiceContract]
//   [OperationContract]

//2) 서비스 객체를 정의 (계약된 인터페이스를 상속)
//   구현상속(상속받은 추상 메서드를 정의)

//3) 호스팅 (IIS or (exe))
//--------------------------------------------------------------------------------

namespace WCF_RESTful
{
    //1. 서비스 계약 선언[인터페이스에서...]
    [ServiceContract]
    public interface IWSUforestService
    {
        #region 브라우저 --> 서비스

        // 로그인 유무 확인
        [OperationContract]
        [WebGet(UriTemplate = "WSU_LoginCheck/{W_id}",
        ResponseFormat = WebMessageFormat.Json)]
        string WSU_LoginCheck(string W_id);

        // 로그인 요청 o
        [OperationContract]
        [WebGet(UriTemplate = "WSU_Login/{W_id}/{password}",
        ResponseFormat = WebMessageFormat.Json)]
        string WSU_Login(string W_id, string password);

        // 로그아웃 요청 o
        [OperationContract]
        [WebGet(UriTemplate = "WSU_Logout/{W_id}",
        ResponseFormat = WebMessageFormat.Json)]
        string WSU_Logout(string W_id);

        // 도서 검색(상세) o
        [OperationContract]
        [WebGet(UriTemplate = "WSU_BookInfoSelect/{b_id}",
        ResponseFormat = WebMessageFormat.Json)]
        WSUlibrary_BookList WSU_BookInfoSelect(string b_id);

        // 도서 검색 o
        [OperationContract]
        [WebGet(UriTemplate = "WSU_BookSelect/{type}/{data}",
        ResponseFormat = WebMessageFormat.Json)]
        List<string> WSU_BookSelect(string type, string data);

        // 사람 대출 목록 o
        [OperationContract]
        [WebGet(UriTemplate = "WSU_BookCheckOutList/{W_id}",
        ResponseFormat = WebMessageFormat.Json)]
        List<string> WSU_BookCheckOutList(string W_id);

        // 사람 찜 목록 o
        [OperationContract]
        [WebGet(UriTemplate = "WSU_BookHeartList/{W_id}",
        ResponseFormat = WebMessageFormat.Json)]
        List<string> WSU_BookHeartList(string W_id);

        // 도서 베스트셀러 목록 o
        [OperationContract]
        [WebGet(UriTemplate = "WSU_BookBestSellerList",
        ResponseFormat = WebMessageFormat.Json)]
        List<string> WSU_BookBestSellerList();

        // 도서 대출 o
        [OperationContract]
        [WebGet(UriTemplate = "WSU_BookCheckOut/{W_id}/{b_id}",
        ResponseFormat = WebMessageFormat.Json)]
        string WSU_BookCheckOut(string W_id, string b_id);

        // 도서 반납
        [OperationContract]
        [WebGet(UriTemplate = "WSU_BookReturn/{W_id}/{b_id}",
        ResponseFormat = WebMessageFormat.Json)]
        string WSU_BookReturn(string W_id, string b_id);

        // 도서 대출 연장
        [OperationContract]
        [WebGet(UriTemplate = "WSU_BookRenew/{W_id}/{b_id}",
        ResponseFormat = WebMessageFormat.Json)]
        string WSU_BookRenew(string W_id, string b_id);

        // 도서 찜 선택
        [OperationContract]
        [WebGet(UriTemplate = "WSU_BookHeart/{W_id}/{b_id}",
        ResponseFormat = WebMessageFormat.Json)]
        string WSU_BookHeart(string W_id, string b_id);

        // 도서 찜 해제
        [OperationContract]
        [WebGet(UriTemplate = "WSU_BookUnHeart/{W_id}/{b_id}",
        ResponseFormat = WebMessageFormat.Json)]
        string WSU_BookUnHeart(string W_id, string b_id);




        #endregion

        #region 유니티 --> 서비스

        // 게임접속 요청
        [OperationContract]
        [WebInvoke(UriTemplate = "Ply_GameJoin",
                   Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest)]

        string Ply_GameJoin(int id);

        // 게임접속종료 요청
        [OperationContract]
        [WebInvoke(UriTemplate = "Ply_GameUnjoin",
                   Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest)]

        string Ply_GameUnjoin(int id);

        // 플레이어 커스텀 저장 및 수정 
        [OperationContract]
        [WebInvoke(UriTemplate = "Ply_UpdateCustom",
                   Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest)]

        string Ply_UpdateCustom(int id, int custom);

        // 북리스트 받아오기 
        [OperationContract]
        [WebInvoke(UriTemplate = "Unity_BookSelect",
                   Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest)]
       
        string Unity_BookSelect(string title, string type);

        // 베스트셀러 받아오기  
        [OperationContract]
        [WebInvoke(UriTemplate = "Unity_BestSeller",
                   Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        List<string> Unity_Bestseller();

        // 찜 목록 가져오기 사용X
        [OperationContract]
        [WebInvoke(UriTemplate = "Unity_BookCheckwishlist",
                   Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string Unity_BookCheckwishlist(string W_id);

        // 찜목록 개수 가져오기 사용X
        [OperationContract]
        [WebInvoke(UriTemplate = "Unity_BookwishlistCount",
                   Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string Unity_BookwishlistCount(string W_id);


        // 찜 추가
        [OperationContract]
        [WebInvoke(UriTemplate = "Unity_AddWish",
                   Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string Unity_AddWish(string W_id, string b_id);

        // 찜 해제
        [OperationContract]
        [WebInvoke(UriTemplate = "Unity_RemoveWish",
                   Method = "POST",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string Unity_RemoveWish(string W_id, string b_id);

        #endregion
    }
}
