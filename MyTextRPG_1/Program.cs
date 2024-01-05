using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.IO;
using System.Text.Json;
using System.Numerics;

class Character
{
    public string Name { get; set; }
    public int Level { get; set; }
    public string Job { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Health { get; set; }
    public int Gold { get; set; }
    public int Exp { get; set; }
    public int MaxExp { get; set; }
}

class Item
{
    public string Name { get; set; }
    public int AtkStat { get; set; }
    public int DefStat { get; set; }
    public string FlavorText { get; set; }
    public int Price { get; set; }
    public string Type { get; set; }
    public bool IsPurchased { get; set; }
    public bool IsEquipped { get; set; }
}

class Program
{
    static void Main(string[] args)
    {

        Character player = LoadPlayerData();
        List<Item> itemArray = LoadItemsData();

        if (player == null || itemArray == null)
        {

            player = new Character
            {
                Name = "막시무스",
                Level = 01,
                Job = "검투사",
                Attack = 10,
                Defense = 5,
                Health = 100,
                Gold = 1500,
                Exp = 0,
                MaxExp = 1
            };

            itemArray = new List<Item>
        {
            new Item { Name = "흔한 검", AtkStat = 2, DefStat = 0, FlavorText = "초심자들의 친구", Price = 200, Type = "Weapon", IsPurchased = true, IsEquipped = true },
            new Item { Name = "나무 방패", AtkStat = 0, DefStat = 9, FlavorText = "그럭저럭 쓸만하다", Price = 150, Type = "Armor", IsPurchased = true, IsEquipped = true },
            new Item { Name = "전투 도끼", AtkStat = 10, DefStat = 0, FlavorText = "뚝배기 브레이커", Price = 1200, Type = "Weapon", IsPurchased = false, IsEquipped = false },
            new Item { Name = "사슬 갑옷", AtkStat = 0, DefStat = 16, FlavorText = "가볍고 단단하다.", Price = 1000, Type = "Armor", IsPurchased = false, IsEquipped = false },
            new Item { Name = "스톰 브링어", AtkStat = 20, DefStat = 0, FlavorText = "폭풍을 부른다고 전해지는 검", Price = 5000, Type = "Weapon", IsPurchased = false, IsEquipped = false },
            new Item { Name = "강철 갑옷", AtkStat = 0, DefStat = 24, FlavorText = "강철로 만든 갑옷", Price = 4500, Type = "Armor", IsPurchased = false, IsEquipped = false },
            new Item { Name = "빛나는 성검", AtkStat = 45, DefStat = 0, FlavorText = "눈부신 빛을 내는 전설의 성검", Price = 12000, Type = "Weapon", IsPurchased = false, IsEquipped = false },
            new Item { Name = "미스릴 아머", AtkStat = 0, DefStat = 36, FlavorText = "절대 부서지지 않는 불멸의 갑옷", Price = 10000, Type = "Armor", IsPurchased = false, IsEquipped = false }
        };
        }
        else
        {
            Console.WriteLine("[데이터 로드 성공.]\n");
        }


        List<Item> haveItemList = new List<Item>();
        foreach (Item item in itemArray)
        {
            if (item.IsPurchased)
            {
                haveItemList.Add(item);
            }
        }

        string roomManager = "robby_room";
        bool running = true;
        bool dungeon_choice_waiting = false;

        while (running)
        {
            if (roomManager == "robby_room")
            {
                Console.WriteLine("\n[스파르타 마을에 오신 여러분 환영합니다.]");
                Console.WriteLine("[이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.]\n");
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전");
                Console.WriteLine("5. 휴식\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int userInput = int.Parse(Console.ReadLine());
                switch (userInput)
                {
                    case 1:
                        roomManager = "status_room";
                        break;
                    case 2:
                        roomManager = "inventory_room";
                        break;
                    case 3:
                        roomManager = "shop_room";
                        break;
                    case 4:
                        roomManager = "dungeon_room";
                        break;
                    case 5:
                        roomManager = "rest_room";
                        break;
                    default:
                        Console.WriteLine("[잘못된 입력입니다.]\n");
                        break;
                }
            }
            else if (roomManager == "status_room")
            {
                DisplayCharacterStatus(player, haveItemList);
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int userInput = int.Parse(Console.ReadLine());
                switch (userInput)
                {
                    case 0:
                        roomManager = "robby_room";
                        break;
                    default:
                        Console.WriteLine("[잘못된 입력입니다.]\n");
                        break;
                }
            }
            else if (roomManager == "inventory_room")
            {
                Console.WriteLine("\n[인벤토리]");
                Console.WriteLine("[보유 중인 아이템을 관리할 수 있습니다.]\n");
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < haveItemList.Count; i++)
                {
                    string equippedTag = haveItemList[i].IsEquipped ? "[E]" : "";
                    string itemType = haveItemList[i].Type == "Weapon" ? "공격력" : (haveItemList[i].Type == "Armor" ? "방어력" : "");
                    int itemStat = haveItemList[i].Type == "Weapon" ? haveItemList[i].AtkStat : (haveItemList[i].Type == "Armor" ? haveItemList[i].DefStat : 0);
                    Console.WriteLine($"- {equippedTag}{haveItemList[i].Name,-15} | {itemType} +{itemStat} | {haveItemList[i].FlavorText}");
                }

                Console.WriteLine("\n1. 장착관리");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int userInput = int.Parse(Console.ReadLine());

                switch (userInput)
                {
                    case 0:
                        roomManager = "robby_room";
                        break;
                    case 1:
                        roomManager = "equipping_item";
                        break;
                    default:
                        Console.WriteLine("[잘못된 입력입니다.]\n");
                        break;
                }
            }
            else if (roomManager == "equipping_item")
            {
                Console.WriteLine("\n[인벤토리 - 장착 관리]");
                Console.WriteLine("[보유 중인 아이템을 관리할 수 있습니다.]\n");
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < haveItemList.Count; i++)
                {
                    Console.Write($"- {i + 1}");
                    string equippedTag = haveItemList[i].IsEquipped ? "[E]" : "";
                    string itemType = haveItemList[i].Type == "Weapon" ? "공격력" : (haveItemList[i].Type == "Armor" ? "방어력" : "");
                    int itemStat = haveItemList[i].Type == "Weapon" ? haveItemList[i].AtkStat : (haveItemList[i].Type == "Armor" ? haveItemList[i].DefStat : 0);
                    Console.WriteLine($" {equippedTag}{haveItemList[i].Name,-15} | {itemType} +{itemStat} | {haveItemList[i].FlavorText}");
                }

                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int userInput = int.Parse(Console.ReadLine());

                if (userInput == 0)
                {
                    roomManager = "inventory_room";
                }
                else if (userInput >= 1 && userInput <= haveItemList.Count)
                {
                    int selectedIndex = userInput - 1;
                    Item selectedItem = haveItemList[selectedIndex];

                    selectedItem.IsEquipped = !selectedItem.IsEquipped;

                    if (selectedItem.IsEquipped)
                    {
                        string itemType = selectedItem.Type;
                        foreach (Item item in haveItemList.Where(item => item.Type == itemType && item != selectedItem && item.IsEquipped))
                        {
                            item.IsEquipped = false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("[잘못된 입력입니다.]\n");
                }
            }
            else if (roomManager == "shop_room")
            {
                Console.WriteLine("\n[상점]");
                Console.WriteLine("[필요한 아이템을 얻을 수 있는 상점입니다.]\n");
                Console.WriteLine($"[보유 골드]\n{player.Gold} G");
                Console.WriteLine("\n[아이템 목록]");
                for (int i = 0; i < itemArray.Count; i++)
                {
                    string purchaseStatus = itemArray[i].IsPurchased ? "구매완료" : $"{itemArray[i].Price} G";
                    Console.WriteLine($"- {itemArray[i].Name,-15} | {GetItemStatInfo(itemArray[i])} | {itemArray[i].FlavorText,-45} |  {purchaseStatus}");
                }

                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("2. 아이템 판매");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int userInput = int.Parse(Console.ReadLine());

                switch (userInput)
                {
                    case 0:
                        roomManager = "robby_room";
                        break;
                    case 1:
                        roomManager = "buying_item";
                        break;
                    case 2:
                        roomManager = "selling_item";
                        break;
                    default:
                        Console.WriteLine("[잘못된 입력입니다.]\n");
                        break;
                }
            }
            else if (roomManager == "buying_item")
            {
                Console.WriteLine("\n[상점 - 아이템 구매]");
                Console.WriteLine("[아이템 번호를 입력하면 아이템을 구매할 수 있습니다.]\n");
                Console.WriteLine($"[보유 골드]\n{player.Gold} G");
                Console.WriteLine("\n[아이템 목록]");
                for (int i = 0; i < itemArray.Count; i++)
                {
                    string purchaseStatus = itemArray[i].IsPurchased ? "구매완료" : $"{itemArray[i].Price} G";
                    Console.WriteLine($"- {i + 1}. {itemArray[i].Name,-15} | {GetItemStatInfo(itemArray[i])} | {itemArray[i].FlavorText,-45} |  {purchaseStatus}");
                }

                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int userInput = int.Parse(Console.ReadLine());

                if (userInput == 0)
                {
                    roomManager = "shop_room";
                }
                else if (userInput >= 1 && userInput <= itemArray.Count)
                {
                    int selectedIndex = userInput - 1;
                    Item selecteditem = itemArray[selectedIndex];

                    if (!selecteditem.IsPurchased)
                    {
                        if (player.Gold >= selecteditem.Price)
                        {
                            selecteditem.IsPurchased = true;
                            haveItemList.Add(selecteditem);
                            player.Gold -= selecteditem.Price;

                            Console.WriteLine($"[{selecteditem.Name}을(를) 구매했습니다. [보유 골드: {player.Gold} G]]\n");
                        }
                        else
                        {
                            Console.WriteLine("[골드가 부족합니다.]\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("[이미 구매한 아이템입니다.]\n");
                    }
                }
                else
                {
                    Console.WriteLine("[잘못된 입력입니다.]\n");
                }
            }
            else if (roomManager == "selling_item")
            {
                Console.WriteLine("\n[상점 - 아이템 판매]");
                Console.WriteLine("[아이템 번호를 입력하면 아이템을 판매할 수 있습니다.]");
                Console.WriteLine("[판매 가격은 아이템 구매 가격의 85%입니다.]\n");
                Console.WriteLine($"[보유 골드]\n{player.Gold} G");
                Console.WriteLine("\n[보유 중인 아이템 목록]");
                for (int i = 0; i < haveItemList.Count; i++)
                {
                    Console.WriteLine($"- {i + 1}. {haveItemList[i].Name,-15} | {GetItemStatInfo(haveItemList[i])} | {haveItemList[i].FlavorText,-45} |  {haveItemList[i].Price * 85 / 100} G");
                }

                Console.WriteLine("\n0. 나가기");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int userInput = int.Parse(Console.ReadLine());

                if (userInput == 0)
                {
                    roomManager = "shop_room";
                }
                else if (userInput >= 1 && userInput <= haveItemList.Count)
                {
                    int selectedIndex = userInput - 1;
                    Item selecteditem = haveItemList[selectedIndex];

                    selecteditem.IsPurchased = false;
                    selecteditem.IsEquipped = false;
                    haveItemList.Remove(selecteditem);
                    player.Gold += (int)(selecteditem.Price * 0.85);

                    Console.WriteLine($"[{selecteditem.Name}을(를) 판매했습니다. [보유 골드: {player.Gold} G]]\n");
                }
                else
                {
                    Console.WriteLine("[잘못된 입력입니다.]\n");
                }
            }

            else if (roomManager == "dungeon_room")
            {
                int totalDef = CalculateTotalDefense(player, haveItemList);
                int totalAtk = CalculateTotalAttack(player, haveItemList);

                Console.WriteLine("\n[던전]");
                Console.WriteLine("[던전에 입장할 때마다 체력이 소모됩니다.]");
                Console.WriteLine("[공격력이 높을 수록 보상 Up, 방어력이 높을 수록 성공 확률 Up]");
                Console.WriteLine("[입장 난이도를 선택하세요.]\n");

                Console.WriteLine($"[현재 공격력 : {totalAtk}]");
                Console.WriteLine($"[현재 방어력 : {totalDef}]");
                Console.WriteLine($"[현재 체력 : {player.Health}]\n");


                Console.WriteLine("1. 쉬운 던전        | 방어력 15 이상 권장" + (totalDef >= 15 ? " [조건 충족!]" : ""));
                Console.WriteLine("2. 일반 던전        | 방어력 25 이상 권장" + (totalDef >= 25 ? " [조건 충족!]" : ""));
                Console.WriteLine("3. 어려운 던전        | 방어력 40 이상 권장" + (totalDef >= 40 ? " [조건 충족!]" : ""));
                Console.WriteLine("\n0. 나가기");
                Console.Write(">>");
                int userInput = int.Parse(Console.ReadLine());

                switch (userInput)
                {
                    case 0:
                        roomManager = "robby_room";
                        break;
                    case 1:
                        roomManager = "easy_dungeon";
                        break;
                    case 2:
                        roomManager = "normal_dungeon";
                        break;
                    case 3:
                        roomManager = "hard_dungeon";
                        break;
                    default:
                        Console.WriteLine("[잘못된 입력입니다.]\n");
                        break;
                }
            }
            else if (roomManager == "easy_dungeon")
            {
                ExploreDungeon(player, haveItemList, ref roomManager, 15, 1000, ref dungeon_choice_waiting);
            }
            else if (roomManager == "normal_dungeon")
            {
                ExploreDungeon(player, haveItemList, ref roomManager, 25, 1700, ref dungeon_choice_waiting);
            }
            else if (roomManager == "hard_dungeon")
            {
                ExploreDungeon(player, haveItemList, ref roomManager, 40, 2500, ref dungeon_choice_waiting);
            }
            else if (roomManager == "rest_room")
            {
                Console.WriteLine("[여관]\n");
                Console.WriteLine($"[500 G를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.Gold} G )]");
                Console.WriteLine($"[여관에서 휴식하면 데이터를 저장할 수 있습니다.]");
                Console.WriteLine($"[휴식하기 (비용: 500 G) 현재 체력: {player.Health}]\n");
                Console.WriteLine("1. 휴식하기");
                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                int restChoice;
                if (int.TryParse(Console.ReadLine(), out restChoice))
                {
                    switch (restChoice)
                    {
                        case 1:
                            if (player.Gold >= 500)
                            {
                                player.Health = 100;
                                player.Gold -= 500;

                                Console.WriteLine($"[체력이 100이 되었습니다.]");
                                SaveData(player, itemArray);
                                Console.WriteLine($"[보유 골드]\n{player.Gold} G");
  
                            }
                            else
                            {
                                Console.WriteLine($"[골드가 부족합니다.]");
                                Console.WriteLine($"[보유 골드]\n{player.Gold} G");
                            }
                            break;
                        case 0:
                            roomManager = "robby_room";
                            break;
                        default:
                            Console.WriteLine("[잘못된 입력입니다.]\n");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("[잘못된 입력입니다.]\n");
                }
            }

            else
            {
                Console.WriteLine("\n[오류가 발생했습니다. 로비로 이동합니다.]\n");
                roomManager = "robby_room";
            }

        }

        AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
        {
            SaveData(player, itemArray);
        };
    }
    static void DisplayCharacterStatus(Character player, List<Item> equippedItems)
    {
        Console.WriteLine("\n[상태 보기]");
        Console.WriteLine("[캐릭터의 정보가 표시됩니다.]\n");
        Console.WriteLine($"Lv. {player.Level:D2}");
        Console.WriteLine($"이름 : {player.Name}");
        Console.WriteLine($"직업 : ({player.Job})");

        int totalAttack = player.Attack;
        var equippedWeapons = equippedItems.Where(item => item.Type == "Weapon" && item.IsEquipped);
        if (equippedWeapons.Any())
        {
            totalAttack += equippedWeapons.Sum(item => item.AtkStat);
            Console.WriteLine($"공격력 : {totalAttack} + ({equippedWeapons.Sum(item => item.AtkStat)})");
        }
        else
        {
            Console.WriteLine($"공격력 : {totalAttack}");
        }
        int totalDefense = player.Defense;
        var equippedArmors = equippedItems.Where(item => item.Type == "Armor" && item.IsEquipped);
        if (equippedArmors.Any())
        {
            totalDefense += equippedArmors.Sum(item => item.DefStat);
            Console.WriteLine($"방어력 : {totalDefense} + ({equippedArmors.Sum(item => item.DefStat)})");
        }
        else
        {
            Console.WriteLine($"방어력 : {totalDefense}");
        }

        Console.WriteLine($"체 력 : {player.Health}");
        Console.WriteLine($"Gold : {player.Gold} G\n");
        Console.WriteLine($"[현재 경험치: {player.Exp} / 최대 경험치: {player.MaxExp}]\n[다음 레벨까지 {player.MaxExp - player.Exp} 남았습니다.]\n");
    }

    static int CalculateTotalDefense(Character player, List<Item> equippedItems)
    {
        int totalDefense = player.Defense;

        var equippedArmors = equippedItems.Where(item => item.Type == "Armor" && item.IsEquipped);
        if (equippedArmors.Any())
        {
            totalDefense += equippedArmors.Sum(item => item.DefStat);
        }

        return totalDefense;
    }

    static int CalculateTotalAttack(Character player, List<Item> equippedItems)
    {
        int totalAttack = player.Attack;
        var equippedWeapons = equippedItems.Where(item => item.Type == "Weapon" && item.IsEquipped);
        if (equippedWeapons.Any())
        {
            totalAttack += equippedWeapons.Sum(item => item.AtkStat);
        }

        return totalAttack;
    }

    static string GetItemStatInfo(Item item)
    {
        if (item.Type == "Weapon")
        {
            return $"공격력 +{item.AtkStat}";
        }
        else if (item.Type == "Armor")
        {
            return $"방어력 +{item.DefStat}";
        }
        else
        {
            return "스탯 정보 없음";
        }
    }

    static void ExploreDungeon(Character player, List<Item> haveItemList, ref string roomManager, int appropriate_def, int basic_reward, ref bool dungeon_choice_waiting)
    {
        if (dungeon_choice_waiting == false)
        {
            if (player.Health <= 0)
            {
                player.Health = 0;
                Console.WriteLine("[체력이 부족합니다.]");
                Console.WriteLine("[체력이 부족하여 던전에 입장할 수 없습니다.]");
                Console.WriteLine("[던전 선택방으로 이동합니다.]\n");
                roomManager = "dungeon_room";
                return;
            }

            int totalDef = CalculateTotalDefense(player, haveItemList);
            int totalAtk = CalculateTotalAttack(player, haveItemList);

            if (totalDef >= appropriate_def)
            {
                int damageTaken = new Random().Next(20 + (appropriate_def - totalDef), 35 + (appropriate_def - totalDef));
                player.Health -= damageTaken;
                if (player.Health <= 0)
                {
                    player.Health = 0;
                }

                int bonusGoldPercentage = new Random().Next(totalAtk, totalAtk * 2);
                int bonusGold = (int)(basic_reward * bonusGoldPercentage / 100.0);
                int goldEarned = basic_reward + bonusGold;
                player.Gold += goldEarned;

                Console.WriteLine($"[던전 클리어! 축하합니다!]\n[체력 감소: {damageTaken}]\n");
                Console.WriteLine($"[기본 보상]\n+{basic_reward} G");
                Console.WriteLine($"[보너스]\n+{bonusGold} G");
                Console.WriteLine($"[총 획득 골드]\n+{goldEarned} G\n");
                Console.WriteLine($"[현재 보유 골드: {player.Gold} G]");
                Console.WriteLine($"[현재 남은 체력: {player.Health}]\n");
                LevelUp(player, haveItemList);

                Console.WriteLine("1. 계속 던전 탐험");
                Console.WriteLine("0. 나가기");
                Console.Write(">>");
                int exploreChoice = int.Parse(Console.ReadLine());

                switch (exploreChoice)
                {
                    case 0:
                        roomManager = "dungeon_room";
                        break;
                    case 1:
                        break;
                    default:
                        Console.WriteLine("[잘못된 입력입니다.]\n");
                        dungeon_choice_waiting = true;
                        break;
                }
            }
            else
            {
                int randomValue = new Random().Next(0, 10);

                if (randomValue <= 3)
                {
                    int damageTaken = new Random().Next(20 + (appropriate_def - totalDef), 35 + (appropriate_def - totalDef)) / 2;
                    player.Health -= damageTaken;
                    if (player.Health <= 0)
                    {
                        player.Health = 0;
                    }

                    Console.WriteLine($"[던전 탐험 실패... 체력 감소: {damageTaken}]\n");
                    Console.WriteLine($"[현재 보유 골드: {player.Gold} G]");
                    Console.WriteLine($"[현재 남은 체력: {player.Health}]\n");

                    Console.WriteLine("1. 다시 시도");
                    Console.WriteLine("0. 나가기");
                    Console.Write(">>");
                    int retryChoice = int.Parse(Console.ReadLine());

                    switch (retryChoice)
                    {
                        case 0:
                            roomManager = "dungeon_room";
                            break;
                        case 1:
                            break;
                        default:
                            Console.WriteLine("[잘못된 입력입니다.]\n");
                            dungeon_choice_waiting = true;
                            break;
                    }
                }
                else
                {
                    int damageTaken = new Random().Next(20 + (appropriate_def - totalDef), 35 + (appropriate_def - totalDef));
                    player.Health -= damageTaken;
                    if (player.Health <= 0)
                    {
                        player.Health = 0;
                    }

                    int bonusGoldPercentage = new Random().Next(totalAtk, totalAtk * 2);
                    int bonusGold = (int)(basic_reward * bonusGoldPercentage / 100.0);
                    int goldEarned = basic_reward + bonusGold;
                    player.Gold += goldEarned;

                    Console.WriteLine($"[던전 클리어! 축하합니다!]\n[체력 감소: {damageTaken}]\n");
                    Console.WriteLine($"[기본 보상]\n+{basic_reward} G");
                    Console.WriteLine($"[보너스]\n+{bonusGold} G");
                    Console.WriteLine($"[총 획득 골드]\n+{goldEarned} G\n");
                    Console.WriteLine($"[현재 보유 골드: {player.Gold} G]");
                    Console.WriteLine($"[현재 남은 체력: {player.Health}]\n");
                    LevelUp(player, haveItemList);

                    Console.WriteLine("1. 계속 던전 탐험");
                    Console.WriteLine("0. 나가기");
                    Console.Write(">>");
                    int exploreChoice = int.Parse(Console.ReadLine());

                    switch (exploreChoice)
                    {
                        case 0:
                            roomManager = "dungeon_room";
                            break;
                        case 1:
                            roomManager = roomManager;
                            break;
                        default:
                            Console.WriteLine("[잘못된 입력입니다.]");
                            dungeon_choice_waiting = true;
                            break;
                    }
                }
            }
        }
        else
        {
            dungeon_choice_waiting = false;
            Console.WriteLine("[올바른 숫자를 입력해주세요.]");
            Console.WriteLine("\n1. 계속 던전 탐험");
            Console.WriteLine("0. 나가기");
            Console.Write(">>");
            int exploreChoice = int.Parse(Console.ReadLine());

            switch (exploreChoice)
            {
                case 0:
                    roomManager = "dungeon_room";
                    break;
                case 1:
                    roomManager = roomManager;
                    break;
                default:
                    Console.WriteLine("[잘못된 입력입니다.]");
                    dungeon_choice_waiting = true;
                    break;
            }
        }
    }
    static void LevelUp(Character player, List<Item> haveItemList)
    {
        player.Exp += 1;
        Console.WriteLine($"[경험치를 1 획득했습니다!]");

        if (player.Exp == player.MaxExp)
        {
            player.Level += 1;
            player.Attack += 1;
            player.Defense += 2;
            player.MaxExp = player.Level;
            player.Exp = 0;

            int totalDef = CalculateTotalDefense(player, haveItemList);
            int totalAtk = CalculateTotalAttack(player, haveItemList);

            Console.WriteLine($"[레벨 업! 축하합니다!]");
            Console.WriteLine($"[레벨 {player.Level} 달성!]\n");
            Console.WriteLine($"[공격력이 1 증가했습니다.]");
            Console.WriteLine($"[방어력이 2 증가했습니다.]\n");
            Console.WriteLine($"[현재 공격력: {totalAtk}]");
            Console.WriteLine($"[현재 방어력: {totalDef}]");
        }

        Console.WriteLine($"[현재 경험치: {player.Exp} / 최대 경험치: {player.MaxExp} 다음 레벨까지 {player.MaxExp - player.Exp} 남았습니다.]\n");
    }

    static void SaveData(Character player, List<Item> items)
    {
        string playerData = JsonSerializer.Serialize(player);
        File.WriteAllText("player_data.json", playerData);

        string itemsData = JsonSerializer.Serialize(items);
        File.WriteAllText("items_data.json", itemsData);

        Console.WriteLine("[데이터를 저장했습니다.]\n");
    }

    static Character LoadPlayerData()
    {
        Console.WriteLine("[데이터 로드 중..]\n");
        if (File.Exists("player_data.json"))
        {
            string playerData = File.ReadAllText("player_data.json");
            return JsonSerializer.Deserialize<Character>(playerData);
        }
        else
        {
            Console.WriteLine("[불러 올 데이터가 없습니다.]\n");
            Console.WriteLine("[새 데이터를 생성 합니다.]\n");
            return null;
        }
    }

    static List<Item> LoadItemsData()
    {
        if (File.Exists("items_data.json"))
        {
            string itemsData = File.ReadAllText("items_data.json");
            return JsonSerializer.Deserialize<List<Item>>(itemsData);
        }
        else
        {
            return null;
        }
    }
}