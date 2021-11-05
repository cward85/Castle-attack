using System;
using System.Collections.Generic;
using System.Text;

namespace CastleAttack
{
    class CastleAI
    {
        public enum PurchaseAction
        {
            None = 0,
            Soldier = 1,
            Sergeant = 2,
            Cavalry = 3,
            Captain = 4,
            Tower = 5,
            ResearchTools = 10,
            ResearchFarming = 11,
            ResearchRoads = 12,
            ResearchEconomy = 13,
            ResearchEfficiency = 14,
            ResearchTraining = 15,
            ResearchBronzeWeapons = 25,
            ResearchIronWeapons = 26,
            ResearchSteelWeapons = 27,
            ResearchBronzeArmor = 35,
            ResearchIronArmor = 36,
            ResearchSteelArmor = 37,
            ResearchSergeant = 45,
            ResearchCaptain = 46,
            ResearchCavalry = 47,
            ResearchTower = 48,
            ResearchBasicMining = 55,
            ResearchGoldPanning = 56,
            ResearchStripMining = 57,
            ResearchAdvancedMining = 58
        }

        public const int BronzeWeaponGold = 2000, IronWeaponGold = 4000, SteelWeaponGold = 8000;
        public const int BronzeArmorGold = 2000, IronArmorGold = 2000, SteelArmorGold = 8000;
        public const int SoldierBuyGold = 100, SergeantBuyGold = 600, CaptainBuyGold = 1000, CavalryBuyGold = 1500, TowerBuyGold = 3000;
        public const int ToolsGold = 500, FarmingGold = 1000, RoadsGold = 1800, EconomyGold = 3000, EfficiencyGold = 5000, TrainingGold = 8000;
        public const int BasicMiningGold = 1000, GoldPanningGold = 2000, StripMiningGold = 4000, AdvancedMiningGold = 8000;
        public const int SergeantResearch = 1000, CaptainResearch = 2000, CavalryResearch = 3500, TowerResearch = 6000;

        bool SergeantUnlocked, CaptainUnlocked, CavalryUnlocked, TowerUnlocked;
        bool BronzeWeapons, IronWeapons, SteelWeapons;
        bool BronzeArmor, IronArmor, SteelArmor;             
        int TroopBuyChance;        
        int ActionsPerTick;
        bool ToolUnlocked, FarmingUnlocked, RoadsUnlocked, EconomyUnlocked, EfficiencyUnlocked, TrainingUnlocked;        
        int TroopLimit;
        int TroopResearch, WeaponResearch, ArmorResearch, ActionResearch, GoldResearch;
        double SoldierChance, SergeantChance, CaptainChance, CavalryChance, TowerChance;
        bool BasicMiningUnlocked, GoldPanningUnlocked, StripMiningUnlocked, AdvancedMiningUnlocked;        
        int MilitaryResearch, EconomyResearch;
        int RandomSeed;

        public CastleAI(int p_intSeed)
        {
            RandomSeed = p_intSeed;

            SergeantUnlocked = false;
            CaptainUnlocked = false;
            CavalryUnlocked = false;
            TowerUnlocked = false;

            BronzeArmor = false;
            IronArmor = false;
            SteelArmor = false;

            BronzeWeapons = false;
            IronWeapons = false;
            SteelWeapons = false;

            ToolUnlocked = false;
            FarmingUnlocked = false;
            RoadsUnlocked = false;
            EconomyUnlocked = false;
            EfficiencyUnlocked = false;
            TrainingUnlocked = false;

            BasicMiningUnlocked = false;
            GoldPanningUnlocked = false;
            StripMiningUnlocked = false;
            AdvancedMiningUnlocked = false;

            SoldierChance = 100;
            SergeantChance = 0;
            CaptainChance = 0;
            CavalryChance = 0;
            TowerChance = 0;

            ActionsPerTick = 2;

            TroopBuyChance = 90;
            TroopLimit = 5;

            Random random = new Random(p_intSeed + DateTime.Now.Second);
            int number = random.Next(0, 8);

            DeterminePercentages(number);
        }

        private void DeterminePercentages(int number)
        {
            if (number == 0)        //military-troop focus
            {
                MilitaryResearch = 75;
                EconomyResearch = 25;
                TroopResearch = 50;
                WeaponResearch = 25;
                ArmorResearch = 25;
                ActionResearch = 50;
                GoldResearch = 50;
            }
            else if (number == 1)       //military-weapon focus
            {
                MilitaryResearch = 75;
                EconomyResearch = 25;
                TroopResearch = 25;
                WeaponResearch = 50;
                ArmorResearch = 25;
                ActionResearch = 50;
                GoldResearch = 50;
            }
            else if (number == 2)       //military-armor focus
            {
                MilitaryResearch = 75;
                EconomyResearch = 25;
                TroopResearch = 25;
                WeaponResearch = 25;
                ArmorResearch = 50;
                ActionResearch = 50;
                GoldResearch = 50;
            }
            else if (number == 3)       //economy-gold focus
            {
                MilitaryResearch = 25;
                EconomyResearch = 75;
                TroopResearch = 33;
                WeaponResearch = 33;
                ArmorResearch = 33;
                ActionResearch = 25;
                GoldResearch = 75;
            }
            else if (number == 4)       //economy-action focus
            {
                MilitaryResearch = 25;
                EconomyResearch = 75;
                TroopResearch = 33;
                WeaponResearch = 33;
                ArmorResearch = 33;
                ActionResearch = 75;
                GoldResearch = 25;
            }
            else if (number == 5)       //balanced focus
            {
                MilitaryResearch = 50;
                EconomyResearch = 50;
                TroopResearch = 34;
                WeaponResearch = 33;
                ArmorResearch = 33;
                ActionResearch = 50;
                GoldResearch = 50;
            }
            else if (number == 6)       //economy-troop focus
            {
                MilitaryResearch = 25;
                EconomyResearch = 75;
                TroopResearch = 50;
                WeaponResearch = 25;
                ArmorResearch = 25;
                ActionResearch = 50;
                GoldResearch = 50;
            }
            else if (number == 7)       //military-gold focus
            {
                MilitaryResearch = 75;
                EconomyResearch = 25;
                TroopResearch = 34;
                WeaponResearch = 33;
                ArmorResearch = 33;
                ActionResearch = 25;
                GoldResearch = 75;
            }
        }

        public int GetActionsPerTick()
        {
            return (ActionsPerTick);
        }

        public void DecreaseBuyTroop(int num)
        {
            TroopBuyChance -= num;
        }

        private PurchaseAction BuyTroops(int troopCount, ref int gold)
        {
            Random random = new Random(DateTime.Now.Second + gold + RandomSeed);
            int number;

            if (troopCount < TroopLimit)
            {
                number = random.Next(0, 100);

                if (number < SoldierChance)
                {
                    if (gold >= SoldierBuyGold)
                    {
                        gold -= SoldierBuyGold;
                        
                        return (PurchaseAction.Soldier);
                    }
                }
                else if (number < SergeantChance)
                {
                    if (gold >= SergeantBuyGold)
                    {
                        gold -= SergeantBuyGold;
                        
                        return (PurchaseAction.Sergeant);
                    }
                }
                else if (number < CavalryChance)
                {
                    if (gold >= CavalryBuyGold)
                    {
                        gold -= CavalryBuyGold;
                        
                        return (PurchaseAction.Cavalry);
                    }
                }
                else if (number < CaptainChance)
                {
                    if (gold >= CaptainBuyGold)
                    {
                        gold -= CaptainBuyGold;
                        
                        return (PurchaseAction.Captain);
                    }
                }
                else if (number < TowerChance)
                {
                    if (gold >= TowerBuyGold)
                    {
                        gold -= TowerBuyGold;
                        
                        return (PurchaseAction.Tower);
                    }
                }
            }

            return (PurchaseAction.None);
        }

        private PurchaseAction ResearchTechnology(ref int gold)
        {
            Random random = new Random(DateTime.Now.Second + gold + RandomSeed);
            int number;
            PurchaseAction returnValue;

            number = random.Next(1, 101);

            if (number < MilitaryResearch)
            {
                number = random.Next(1, 101);

                if (number < TroopResearch)
                {
                    returnValue = ResearchTroops(ref gold);
                   
                    return (returnValue);
                }
                else if (number < TroopResearch + WeaponResearch)
                {
                    returnValue = ResearchWeapons(ref gold);
                    
                    return (returnValue);
                }
                else if (number < TroopResearch + WeaponResearch + ArmorResearch)
                {
                    returnValue = ResearchArmor(ref gold);
                   
                    return (returnValue);

                }
            }
            else if (number < MilitaryResearch + EconomyResearch)
            {
                number = random.Next(1, 101);

                if (number < ActionResearch)
                {
                    returnValue = ResearchAction(ref gold);

                    return (returnValue);
                }
                else if (number < ActionResearch + GoldResearch)
                {
                    returnValue = ResearchGold(ref gold);

                    return (returnValue);
                }
            }

            return (PurchaseAction.None);
        }

        private PurchaseAction ResearchWeapons(ref int gold)
        {
            if (BronzeWeapons == false)
            {
                if (gold >= BronzeWeaponGold)
                {
                    BronzeWeapons = true;

                    gold -= BronzeWeaponGold;

                    return (PurchaseAction.ResearchBronzeWeapons);
                }
            }
            else if (IronWeapons == false && BasicMiningUnlocked)
            {
                if (gold >= IronWeaponGold)
                {
                    IronWeapons = true;

                    gold -= IronWeaponGold;

                    return (PurchaseAction.ResearchIronWeapons);
                }
            }
            else if (SteelWeapons == false && AdvancedMiningUnlocked)
            {
                if (gold >= SteelWeaponGold)
                {
                    SteelWeapons = true;

                    gold -= SteelWeaponGold;

                    return (PurchaseAction.ResearchSteelWeapons);
                }
            }
            
            return (PurchaseAction.None);
        }

        private PurchaseAction ResearchArmor(ref int gold)
        {
            if (BronzeArmor == false)
            {
                if (gold >= BronzeArmorGold)
                {
                    BronzeArmor = true;

                    gold -= BronzeArmorGold;

                    return (PurchaseAction.ResearchBronzeArmor);
                }
            }
            else if (IronArmor == false && BasicMiningUnlocked)
            {
                if (gold >= IronArmorGold)
                {
                    IronArmor = true;

                    gold -= IronArmorGold;

                    return (PurchaseAction.ResearchIronArmor);
                }
            }
            else if (SteelArmor == false && AdvancedMiningUnlocked)
            {
                if (gold >= SteelArmorGold)
                {
                    SteelArmor = true;

                    gold -= SteelArmorGold;

                    return (PurchaseAction.ResearchSteelArmor);
                }
            }
           
            return (PurchaseAction.None);
        }
    
        private PurchaseAction ResearchAction(ref int gold)
        {
            if (ToolUnlocked == false)
            {
                if (gold >= ToolsGold)
                {
                    ToolUnlocked = true;

                    gold -= ToolsGold;
                    ActionsPerTick++;

                    return (PurchaseAction.ResearchTools);
                }
            }
            else if (FarmingUnlocked == false)
            {
                if (gold >= FarmingGold)
                {
                    FarmingUnlocked = true;

                    gold -= FarmingGold;
                    ActionsPerTick++;
                    TroopLimit += 2;

                    return (PurchaseAction.ResearchFarming);
                }
            }
            else if (RoadsUnlocked == false)
            {
                if (gold >= RoadsGold)
                {
                    RoadsUnlocked = true;

                    gold -= RoadsGold;
                    ActionsPerTick++;
                    TroopLimit += 2;

                    return (PurchaseAction.ResearchRoads);
                }
            }
            else if (EconomyUnlocked == false)
            {
                if (gold >= EconomyGold)
                {
                    EconomyUnlocked = true;

                    gold -= EconomyGold;
                    ActionsPerTick++;
                    TroopLimit += 1;

                    return (PurchaseAction.ResearchEconomy);
                }
            }
            else if (EfficiencyUnlocked == false)
            {
                if (gold >= EfficiencyGold)
                {
                    EfficiencyUnlocked = true;

                    gold -= EfficiencyGold;
                    ActionsPerTick++;
                    TroopLimit += 1;

                    return (PurchaseAction.ResearchEfficiency);
                }
            }
            else if (TrainingUnlocked == false)
            {
                if (gold >= TrainingGold)
                {
                    TrainingUnlocked = true;

                    gold -= TrainingGold;
                    ActionsPerTick++;
                    TroopLimit += 5;

                    return (PurchaseAction.ResearchTraining);
                }
            }
          
            return (PurchaseAction.None);
        }     

        private PurchaseAction ResearchTroops(ref int gold)
        {
            if (SergeantUnlocked == false)
            {
                if (gold >= SergeantResearch)
                {
                    gold -= SergeantResearch;                    
                    SergeantUnlocked = true;
                    SergeantChance = 100;
                    SoldierChance = 90;
                    
                    return (PurchaseAction.ResearchSergeant);
                }
            }
            else if (CavalryUnlocked == false)
            {
                if (gold >= CavalryResearch)
                {
                    gold -= CavalryResearch;                  
                    CavalryUnlocked = true;
                    SergeantChance = 95;
                    SoldierChance = 85;
                    CavalryChance = 100;
                    
                    return (PurchaseAction.ResearchCavalry);
                }
            }
            else if (CaptainUnlocked == false)
            {
                if (gold >= CaptainResearch)
                {
                    gold -= CaptainResearch;                   
                    CaptainUnlocked = true;
                    SergeantChance = 85;
                    SoldierChance = 70;
                    CavalryChance = 95;
                    CaptainChance = 100;
                   
                    return (PurchaseAction.ResearchCaptain);
                }
            }
            else if (TowerUnlocked == false)
            {
                if (gold >= TowerResearch)
                {
                    gold -= TowerResearch;                 
                    TowerUnlocked = true;
                    SergeantChance = 83;
                    SoldierChance = 68;
                    CavalryChance = 93;
                    CaptainChance = 98;
                    TowerChance = 100;
                   
                    return (PurchaseAction.ResearchTower);
                }
            }
          
            return (PurchaseAction.None);
        }

        private PurchaseAction ResearchGold(ref int gold)
        {
            if (BasicMiningUnlocked == false)
            {
                if (gold >= BasicMiningGold)
                {
                    BasicMiningUnlocked = true;

                    gold -= BasicMiningGold;
                    
                    return (PurchaseAction.ResearchBasicMining);
                }
            }
            else if (GoldPanningUnlocked == false)
            {
                if (gold >= GoldPanningGold)
                {
                    GoldPanningUnlocked = true;

                    gold -= GoldPanningGold;
                   
                    return (PurchaseAction.ResearchGoldPanning);
                }
            }
            else if (StripMiningUnlocked == false)
            {
                if (gold >= StripMiningGold)
                {
                    StripMiningUnlocked = true;

                    gold -= StripMiningGold;
                   
                    return (PurchaseAction.ResearchStripMining);
                }
            }
            else if (AdvancedMiningUnlocked == false)
            {
                if (gold >= AdvancedMiningGold)
                {
                    AdvancedMiningUnlocked = true;

                    gold -= AdvancedMiningGold;
                  
                    return (PurchaseAction.ResearchAdvancedMining);
                }
            }           

            return (PurchaseAction.None);
        }

        public PurchaseAction DetermineBuy(int troopCount, ref int gold, int tickCount)
        {
            Random random = new Random(tickCount + RandomSeed + gold + troopCount);
            int number;
            int actions = ActionsPerTick;
                        
            number = random.Next(0, 100);

            if (number < TroopBuyChance)
            {
                return( BuyTroops(troopCount, ref gold));
                
            }
            else
            {
                return( ResearchTechnology(ref gold));
            }            
        }
    }
}
