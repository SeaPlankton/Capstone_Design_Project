using UnityEngine;


public class DataManager : MonoBehaviour
{
    //전체 무기의 정보들을 Load 하기위함
    public ItemDataList ItemList;

    private void Start()
    {
        SaveDB();
    }
    //DB 저장을 위한 모든 함수
    public void SaveDB()
    {
        SaveFirstItemDB();
    }

    //초기 아이템 정보 저장
    public void SaveFirstItemDB()
    {
        ItemList = new ItemDataList();
        ItemList.ItemList = new Item[37];

        Item itemDB = new Item();
        itemDB.ItemName = "권총";
        itemDB.ItemType = "무기";
        itemDB.ItemRarity = "Normal";
        itemDB.Requirement = false;
        itemDB.Reduplicatable = true;
        itemDB.ItemNumber = 0;
        itemDB.ItemDescription = "권총 무기 습득 및 업그레이드";

        Item itemDB1 = new Item();
        itemDB1.ItemName = "돌격 소총";
        itemDB1.ItemType = "무기";
        itemDB1.ItemRarity = "Normal";
        itemDB1.Requirement = false;
        itemDB1.Reduplicatable = true;
        itemDB1.ItemNumber = 1;
        itemDB1.ItemDescription = "돌격 소총 무기 습득 및 업그레이드";

        Item itemDB2 = new Item();
        itemDB2.ItemName = "산탄총";
        itemDB2.ItemType = "무기";
        itemDB2.ItemRarity = "Normal";
        itemDB2.Requirement = false;
        itemDB2.Reduplicatable = true;
        itemDB2.ItemNumber = 2;
        itemDB2.ItemDescription = "산탄총 무기 습득 및 업그레이드";

        Item itemDB3 = new Item();
        itemDB3.ItemName = "저격 소총";
        itemDB3.ItemType = "무기";
        itemDB3.ItemRarity = "Normal";
        itemDB3.Requirement = false;
        itemDB3.Reduplicatable = true;
        itemDB3.ItemNumber = 3;
        itemDB3.ItemDescription = "저격 소총 무기 습득 및 업그레이드";

        Item itemDB4 = new Item();
        itemDB4.ItemName = "바주카포";
        itemDB4.ItemType = "무기";
        itemDB4.ItemRarity = "Normal";
        itemDB4.Requirement = false;
        itemDB4.Reduplicatable = true;
        itemDB4.ItemNumber = 4;
        itemDB4.ItemDescription = "바주카포 무기 습득 및 업그레이드";

        Item itemDB5 = new Item();
        itemDB5.AccessoryWeaponType = new string[6];
        itemDB5.ItemName = "FMJ";
        itemDB5.ItemType = "액세서리";
        itemDB5.ItemRarity = "Normal";
        itemDB5.Requirement = false;
        itemDB5.Reduplicatable = true;
        itemDB5.ItemNumber = 5;
        itemDB5.ItemDescription = "탄약 업그레이드 - 데미지 2 증가";
        itemDB5.AccessoryWeaponType[0] = "권총";
        itemDB5.AccessoryWeaponType[1] = "돌격 소총";
        itemDB5.AccessoryWeaponType[2] = "산탄총";
        itemDB5.AccessoryWeaponType[3] = "저격 소총";
        itemDB5.AccessoryWeaponType[4] = "바주카포";
        itemDB5.AccessoryWeaponType[5] = "리볼버형 산탄총";

        Item itemDB6 = new Item();
        itemDB6.AccessoryWeaponType = new string[3];
        itemDB6.ItemName = "HP";
        itemDB6.ItemType = "액세서리";
        itemDB6.ItemRarity = "Normal";
        itemDB6.Requirement = false;
        itemDB6.Reduplicatable = true;
        itemDB6.ItemNumber = 6;
        itemDB6.ItemDescription = "탄약 업그레이드 - 데미지 10 증가 (권총 및 돌격소총 전용)";
        itemDB6.AccessoryWeaponType[0] = "권총";
        itemDB6.AccessoryWeaponType[1] = "돌격 소총";
        itemDB6.AccessoryWeaponType[2] = "리볼버형 산탄총";

        Item itemDB7 = new Item();
        itemDB7.AccessoryWeaponType = new string[5];
        itemDB7.ItemName = "Magnum";
        itemDB7.ItemType = "액세서리";
        itemDB7.ItemRarity = "Normal";
        itemDB7.Requirement = false;
        itemDB7.Reduplicatable = true;
        itemDB7.ItemNumber = 7;
        itemDB7.ItemDescription = "탄약 업그레이드 - 데미지 10 증가";
        itemDB7.AccessoryWeaponType[0] = "권총";
        itemDB7.AccessoryWeaponType[1] = "돌격 소총";
        itemDB7.AccessoryWeaponType[2] = "저격 소총";
        itemDB7.AccessoryWeaponType[3] = "산탄총";
        itemDB7.AccessoryWeaponType[4] = "리볼버형 산탄총";

        Item itemDB8 = new Item();
        itemDB8.AccessoryWeaponType = new string[2];
        itemDB8.ItemName = "AP";
        itemDB8.ItemType = "액세서리";
        itemDB8.ItemRarity = "Normal";
        itemDB8.Requirement = false;
        itemDB8.ItemNumber = 8;
        itemDB8.ItemDescription = "탄약 업그레이드 - 돌격소총 관통 기능, 데미지 20 증가";
        itemDB8.AccessoryWeaponType[0] = "저격 소총";
        itemDB8.AccessoryWeaponType[1] = "돌격 소총";

        Item itemDB9 = new Item();
        itemDB9.AccessoryWeaponType = new string[3];
        itemDB9.ItemName = "Flechette";
        itemDB9.ItemType = "액세서리";
        itemDB9.ItemRarity = "Normal";
        itemDB9.Requirement = false;
        itemDB9.Reduplicatable = false;
        itemDB9.ItemNumber = 9;
        itemDB9.ItemDescription = "탄약 업그레이드 - 권총 및 샷건에 관통 기능, 데미지 5 감소";
        itemDB9.AccessoryWeaponType[0] = "권총";
        itemDB9.AccessoryWeaponType[1] = "산탄총";
        itemDB9.AccessoryWeaponType[2] = "리볼버형 산탄총";

        Item itemDB10 = new Item();
        itemDB10.AccessoryWeaponType = new string[3];
        itemDB10.ItemName = "000 Buckshot";
        itemDB10.ItemType = "액세서리";
        itemDB10.ItemRarity = "Normal";
        itemDB10.Requirement = false;
        itemDB10.Reduplicatable = false;
        itemDB10.ItemNumber = 10;
        itemDB10.ItemDescription = "권총 및 샷건에 관통 제거, 데미지 5 증가";
        itemDB10.AccessoryWeaponType[0] = "권총";
        itemDB10.AccessoryWeaponType[1] = "산탄총";
        itemDB10.AccessoryWeaponType[2] = "리볼버형 산탄총";

        Item itemDB11 = new Item();
        itemDB11.AccessoryWeaponType = new string[1];
        itemDB11.ItemName = "저압 고폭탄";
        itemDB11.ItemType = "액세서리";
        itemDB11.ItemRarity = "Normal";
        itemDB11.Requirement = false;
        itemDB11.Reduplicatable = false;
        itemDB11.ItemNumber = 11;
        itemDB11.ItemDescription = "탄약 업그레이드 - 바주카포 데미지 30 증가 / 폭발 범위 30% 증가";
        itemDB11.AccessoryWeaponType[0] = "바주카포";

        Item itemDB12 = new Item();
        itemDB12.AccessoryWeaponType = new string[6];
        itemDB12.ItemName = "LCAA 6251";
        itemDB12.ItemType = "액세서리";
        itemDB12.ItemRarity = "Normal";
        itemDB12.Requirement = false;
        itemDB12.Reduplicatable = true;
        itemDB12.ItemNumber = 12;
        itemDB12.ItemDescription = "HP 50 즉시 소비, 모든 데미지 25 증가";
        itemDB12.AccessoryWeaponType[0] = "권총";
        itemDB12.AccessoryWeaponType[1] = "돌격 소총";
        itemDB12.AccessoryWeaponType[2] = "산탄총";
        itemDB12.AccessoryWeaponType[3] = "저격 소총";
        itemDB12.AccessoryWeaponType[4] = "바주카포";
        itemDB12.AccessoryWeaponType[5] = "리볼버형 산탄총";

        Item itemDB13 = new Item();
        itemDB13.AccessoryWeaponType = new string[1];
        itemDB13.ItemName = "쵸크 1";
        itemDB13.ItemType = "액세서리";
        itemDB13.ItemRarity = "Normal";
        itemDB13.Requirement = false;
        itemDB13.Reduplicatable = false;
        itemDB13.ItemNumber = 13;
        itemDB13.ItemDescription = "산탄총의 탄 퍼짐을 줄여준다. (산탄총 전용)";
        itemDB13.AccessoryWeaponType[0] = "산탄총";

        Item itemDB14 = new Item();
        itemDB14.AccessoryWeaponType = new string[1];
        itemDB14.ItemName = "도날드덕";
        itemDB14.ItemType = "액세서리";
        itemDB14.ItemRarity = "Normal";
        itemDB14.Requirement = false;
        itemDB14.Reduplicatable = false;
        itemDB14.ItemNumber = 14;
        itemDB14.ItemDescription = "산탄총의 탄 퍼짐 각도를 좌우 10도 높혀준다. (산탄총 전용)";
        itemDB14.AccessoryWeaponType[0] = "산탄총";

        Item itemDB15 = new Item();
        itemDB15.AccessoryWeaponType = new string[1];
        itemDB15.ItemName = "그레이즈1";
        itemDB15.ItemType = "액세서리";
        itemDB15.ItemRarity = "Normal";
        itemDB15.Requirement = false;
        itemDB15.Reduplicatable = false;
        itemDB15.ItemNumber = 15;
        itemDB15.ItemDescription =
            "저격 소총의 탄속을 강화시켜 주변에 피해를 끼치게 한다. (저격 소총 전용)";
        itemDB15.AccessoryWeaponType[0] = "저격 소총";

        Item itemDB16 = new Item();
        itemDB16.AccessoryWeaponType = new string[1];
        itemDB16.ItemName = "그레이즈2";
        itemDB16.ItemType = "액세서리";
        itemDB16.ItemRarity = "Normal";
        itemDB16.Requirement = true;
        itemDB16.Reduplicatable = false;
        itemDB16.ItemNumber = 16;
        itemDB16.ItemDescription =
            "저격 소총의 탄속을 강화시켜 주변에 강력한 피해를 끼치게 한다. (저격 소총 전용)";
        itemDB16.AccessoryWeaponType[0] = "저격 소총";

        Item itemDB17 = new Item();
        itemDB17.AccessoryWeaponType = new string[1];
        itemDB17.ItemName = "페인트 탄";
        itemDB17.ItemType = "액세서리";
        itemDB17.ItemRarity = "Normal";
        itemDB17.Requirement = false;
        itemDB17.Reduplicatable = false;
        itemDB17.ItemNumber = 17;
        itemDB17.ItemDescription =
            "끈적한 탄환을 발사하여 적들의 이동을 방해합니다. (저격 소총 전용)";
        itemDB17.AccessoryWeaponType[0] = "저격 소총";

        Item itemDB18 = new Item();
        itemDB18.AccessoryWeaponType = new string[2];
        itemDB18.ItemName = "아킴보";
        itemDB18.ItemType = "액세서리";
        itemDB18.ItemRarity = "Normal";
        itemDB18.Requirement = false;
        itemDB18.Reduplicatable = false;
        itemDB18.ItemNumber = 18;
        itemDB18.ItemDescription =
            "소총을 양 손에 나란히 들고 다니게 됩니다. (권총 및 돌격 소총 전용)";
        itemDB18.AccessoryWeaponType[0] = "권총";
        itemDB18.AccessoryWeaponType[1] = "돌격 소총";

        Item itemDB19 = new Item();
        itemDB19.AccessoryWeaponType = new string[3];
        itemDB19.ItemName = "소음기";
        itemDB19.ItemType = "액세서리";
        itemDB19.ItemRarity = "Normal";
        itemDB19.Requirement = false;
        itemDB19.Reduplicatable = false;
        itemDB19.ItemNumber = 19;
        itemDB19.ItemDescription =
            "총기에 소음기를 달아 무기의 안정감을 더해줍니다. (권총 및 돌격 소총 전용)";
        itemDB19.AccessoryWeaponType[0] = "권총";
        itemDB19.AccessoryWeaponType[1] = "돌격 소총";
        itemDB19.AccessoryWeaponType[2] = "리볼버형 산탄총";

        Item itemDB20 = new Item();
        itemDB20.AccessoryWeaponType = new string[3];
        itemDB20.ItemName = "소믈리에의 추천";
        itemDB20.ItemType = "액세서리";
        itemDB20.ItemRarity = "Normal";
        itemDB20.Requirement = false;
        itemDB20.Reduplicatable = false;
        itemDB20.ItemNumber = 20;
        itemDB20.ItemDescription =
            "당신의 무기에 소음기가 달려있다면, 묵직하고 깔끔한... 누군가의 은총을 깃들게 합니다. (권총 및 돌격 소총 전용)";
        itemDB20.AccessoryWeaponType[0] = "권총";
        itemDB20.AccessoryWeaponType[1] = "돌격 소총";
        itemDB20.AccessoryWeaponType[2] = "리볼버형 산탄총";

        Item itemDB21 = new Item();
        itemDB21.ItemName = "투척용 나이프";
        itemDB21.ItemType = "무기";
        itemDB21.ItemRarity = "Normal";
        itemDB21.Requirement = false;
        itemDB21.Reduplicatable = false;
        itemDB21.ItemNumber = 21;
        itemDB21.ItemDescription = "투척 나이프 무기 습득";

        Item itemDB22 = new Item();
        itemDB22.ItemName = "Javelin";
        itemDB22.ItemType = "무기";
        itemDB22.ItemRarity = "Normal";
        itemDB22.Requirement = false;
        itemDB22.Reduplicatable = false;
        itemDB22.ItemNumber = 22;
        itemDB22.ItemDescription = "투창 무기 습득";

        Item itemDB23 = new Item();
        itemDB23.ItemName = "신속신 Lv.1";
        itemDB23.ItemType = "액세서리";
        itemDB23.ItemRarity = "Normal";
        itemDB23.Requirement = false;
        itemDB23.Reduplicatable = false;
        itemDB23.ItemNumber = 23;
        itemDB23.ItemDescription = "플레이어의 이동속도를 증가시켜준다.";

        Item itemDB24 = new Item();
        itemDB24.ItemName = "신속신 Lv.2";
        itemDB24.ItemType = "액세서리";
        itemDB24.ItemRarity = "Normal";
        itemDB24.Requirement = true;
        itemDB24.Reduplicatable = false;
        itemDB24.ItemNumber = 24;
        itemDB24.ItemDescription = "플레이어의 이동속도를 증가시켜준다.";

        Item itemDB25 = new Item();
        itemDB25.ItemName = "신속신 Lv.3";
        itemDB25.ItemType = "액세서리";
        itemDB25.ItemRarity = "Normal";
        itemDB25.Requirement = true;
        itemDB25.Reduplicatable = false;
        itemDB25.ItemNumber = 25;
        itemDB25.ItemDescription = "플레이어의 이동속도를 증가시켜준다.";

        Item itemDB26 = new Item();
        itemDB26.AccessoryWeaponType = new string[6];
        itemDB26.ItemName = "불제봉";
        itemDB26.ItemType = "액세서리";
        itemDB26.ItemRarity = "Unique";
        itemDB26.Requirement = false;
        itemDB26.Reduplicatable = false;
        itemDB26.ItemNumber = 26;
        itemDB26.ItemDescription = "누군가의 축복으로 사용자의 신체를 강화 및 회복시켜줍니다.";
        itemDB26.AccessoryWeaponType[0] = "권총";
        itemDB26.AccessoryWeaponType[1] = "돌격 소총";
        itemDB26.AccessoryWeaponType[2] = "산탄총";
        itemDB26.AccessoryWeaponType[3] = "저격 소총";
        itemDB26.AccessoryWeaponType[4] = "바주카포";
        itemDB26.AccessoryWeaponType[5] = "리볼버형 산탄총";

        Item itemDB27 = new Item();
        itemDB27.AccessoryWeaponType = new string[6];
        itemDB27.ItemName = "팔괘로";
        itemDB27.ItemType = "액세서리";
        itemDB27.ItemRarity = "Unique";
        itemDB27.Requirement = false;
        itemDB27.Reduplicatable = false;
        itemDB27.ItemNumber = 27;
        itemDB27.ItemDescription = "누군가의 응원으로 사용자의 데미지가 대폭 강화됩니다.";
        itemDB27.AccessoryWeaponType[0] = "권총";
        itemDB27.AccessoryWeaponType[1] = "돌격 소총";
        itemDB27.AccessoryWeaponType[2] = "산탄총";
        itemDB27.AccessoryWeaponType[3] = "저격 소총";
        itemDB27.AccessoryWeaponType[4] = "바주카포";
        itemDB27.AccessoryWeaponType[5] = "리볼버형 산탄총";

        Item itemDB28 = new Item();
        itemDB28.ItemName = "회중시계";
        itemDB28.ItemType = "액세서리";
        itemDB28.ItemRarity = "Unique";
        itemDB28.Requirement = false;
        itemDB28.Reduplicatable = false;
        itemDB28.ItemNumber = 28;
        itemDB28.ItemDescription = "누군가의 보조로 사용자에게 특수 능력이 부여됩니다.";

        Item itemDB29 = new Item();
        itemDB29.ItemName = "부적";
        itemDB29.ItemType = "액세서리";
        itemDB29.ItemRarity = "Unique";
        itemDB29.Requirement = false;
        itemDB29.Reduplicatable = false;
        itemDB29.ItemNumber = 29;
        itemDB29.ItemDescription = "누군가의 보조로 사용자에게 특수 능력이 부여됩니다.";

        Item itemDB30 = new Item();
        itemDB30.ItemName = "유령검";
        itemDB30.ItemType = "액세서리";
        itemDB30.ItemRarity = "Unique";
        itemDB30.Requirement = false;
        itemDB30.Reduplicatable = false;
        itemDB30.ItemNumber = 30;
        itemDB30.ItemDescription = "누군가의 도움으로 사용자의 활력이 대폭 증가됩니다.";

        Item itemDB31 = new Item();
        itemDB31.ItemName = "전술 교본";
        itemDB31.ItemType = "액세서리";
        itemDB31.ItemRarity = "Unique";
        itemDB31.Requirement = false;
        itemDB31.Reduplicatable = false;
        itemDB31.ItemNumber = 31;
        itemDB31.ItemDescription = "무기를 버려도 기존에 쌓아둔 레벨들이 초기화 되지 않습니다.";

        Item itemDB32 = new Item();
        itemDB32.ItemName = "전술 가방";
        itemDB32.ItemType = "액세서리";
        itemDB32.ItemRarity = "Unique";
        itemDB32.Requirement = false;
        itemDB32.Reduplicatable = false;
        itemDB32.ItemNumber = 32;
        itemDB32.ItemDescription = "소지 가능한 총기의 갯수가 1 증가합니다.";

        Item itemDB33 = new Item();
        itemDB33.ItemName = "메시아의 스완송";
        itemDB33.ItemType = "액세서리";
        itemDB33.ItemRarity = "Unique";
        itemDB33.Requirement = false;
        itemDB33.Reduplicatable = false;
        itemDB33.ItemNumber = 33;
        itemDB33.ItemDescription = "사망 시, 라스트 댄스 5초가 주어집니다. 이때 35명의 길동무를 만들 시, 길동무의 목숨을 대가로 당신이 부활합니다.";

        Item itemDB34 = new Item();
        itemDB34.ItemName = "구급상자 소";
        itemDB34.ItemNumber = 34;
        itemDB34.ItemDescription = "HP를 30 회복하는 아이템이다.";

        Item itemDB35 = new Item();
        itemDB35.ItemName = "구급상자 중";
        itemDB35.ItemNumber = 35;
        itemDB35.ItemDescription = "HP를 70 회복하는 아이템이다.";

        Item itemDB36 = new Item();
        itemDB36.ItemName = "구급상자 대";
        itemDB36.ItemNumber = 36;
        itemDB36.ItemDescription = "HP를 100 회복하는 아이템이다.";

        ItemList.ItemList[0] = itemDB;
        ItemList.ItemList[1] = itemDB1;
        ItemList.ItemList[2] = itemDB2;
        ItemList.ItemList[3] = itemDB3;
        ItemList.ItemList[4] = itemDB4;
        ItemList.ItemList[5] = itemDB5;
        ItemList.ItemList[6] = itemDB6;
        ItemList.ItemList[7] = itemDB7;
        ItemList.ItemList[8] = itemDB8;
        ItemList.ItemList[9] = itemDB9;
        ItemList.ItemList[10] = itemDB10;
        ItemList.ItemList[11] = itemDB11;
        ItemList.ItemList[12] = itemDB12;
        ItemList.ItemList[13] = itemDB13;
        ItemList.ItemList[14] = itemDB14;
        ItemList.ItemList[15] = itemDB15;
        ItemList.ItemList[16] = itemDB16;
        ItemList.ItemList[17] = itemDB17;
        ItemList.ItemList[18] = itemDB18;
        ItemList.ItemList[19] = itemDB19;
        ItemList.ItemList[20] = itemDB20;
        ItemList.ItemList[21] = itemDB21;
        ItemList.ItemList[22] = itemDB22;
        ItemList.ItemList[23] = itemDB23;
        ItemList.ItemList[24] = itemDB24;
        ItemList.ItemList[25] = itemDB25;
        ItemList.ItemList[26] = itemDB26;
        ItemList.ItemList[27] = itemDB27;
        ItemList.ItemList[28] = itemDB28;
        ItemList.ItemList[29] = itemDB29;
        ItemList.ItemList[30] = itemDB30;
        ItemList.ItemList[31] = itemDB31;
        ItemList.ItemList[32] = itemDB32;
        ItemList.ItemList[33] = itemDB33;
        ItemList.ItemList[34] = itemDB34;
        ItemList.ItemList[35] = itemDB35;
        ItemList.ItemList[36] = itemDB36;
    }
}
