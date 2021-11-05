using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using static CastleAttack.CastleAI;

namespace CastleAttack
{
    class Castle : BaseObject
    {
        int gold, goldPerTick, troopCount, hp;
        int bonusAttack, bonusDefense;
        int soldierCost, sergeantCost, cavalryCost, captainCost;
        CastleAI AI;

        public Castle(float x, float y, Color clr, float h, float w, string nam)
        {
            xPos = x;
            yPos = y;
            color = clr;
            height = h;
            width = w;
            name = nam;
            troopCount = 0;
            bonusDefense = 0;
            bonusAttack = 0;
            soldierCost = SoldierBuyGold;
            sergeantCost = SergeantBuyGold;
            cavalryCost = CavalryBuyGold;
            captainCost = CaptainBuyGold;

            gold = 1000;
            goldPerTick = 15;
            hp = 1000;

            AI = new CastleAI((int)x + (int)y + (int)h + (int)w + DateTime.Now.Second);
        }

        public int GetActionsPerTick()
        {
            return( AI.GetActionsPerTick());
        }

        public PurchaseAction DetermineBuy(int tickCount)
        {
            return (AI.DetermineBuy(troopCount, ref gold, tickCount + gold));
        }

        public int GetGold()
        {
            return (gold);
        }

        public int GetHp()
        {
            return (hp);
        }

        public int GetGoldPerTick()
        {
            return (goldPerTick);
        }

        public int GetTroopCount()
        {
            return (troopCount);
        }

        public int GetBonusAttack()
        {
            return (bonusAttack);
        }

        public int GetBonusDefense()
        {
            return (bonusDefense);
        }

        public int GetSoldierCost()
        {
            return (soldierCost);
        }

        public int GetSergeantCost()
        {
            return (sergeantCost);
        }

        public int GetCavalryCost()
        {
            return (cavalryCost);
        }

        public int GetCaptainCost()
        {
            return (captainCost);
        }

        public void DecrementHP(int val)
        {
            hp -= val;
        }

        public void IncrementTroopCount()
        {
            troopCount++;
        }

        public void DecrementTroopCount()
        {
            troopCount--;
        }

        public void DecreaseBuyTroop(int num)
        {
            AI.DecreaseBuyTroop(num);
        }

        public bool SetGold(int value)
        {
            if (gold + value >= 0)
            {
                gold += value;

                return (true);
            }

            return (false);
        }

        public void AddGoldPerTick(int val)
        {
            goldPerTick += val;
        }

        public void IncreaseTroopAttack(int amount)
        {
            bonusAttack += amount;
        }

        public void IncreaseTroopDefense(int amount)
        {
            bonusDefense += amount;
        }

        public void IncrementUnitCosts(int soldier, int sergeant, int cavalry, int captain)
        {
            soldierCost += soldier;
            sergeantCost += sergeant;
            cavalryCost += cavalry;
            captainCost += captain;
        }       
    }
}
