using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using static CastleAttack.CastleAI;
using static CastleAttack.Soldier;
using System.Linq;

namespace CastleAttack
{
    public partial class CastleAttack : Form
    {
        int tickModifier;
        List<Soldier> BlueSoldierList;
        List<Soldier> RedSoldierList;
     
        Castle BlueCastle;
        Castle RedCastle;       
        int tickCount, battleOffset;
        double castleBonus;
        public static int BaseID = 0;

        public CastleAttack()
        {
            InitializeComponent();

            float castleSize = 50;
            float castleYPos = this.Height / 2 + castleSize / 2 - tbEvents.Size.Height;
            tickModifier = 7;
            tickCount = 0;
            this.DoubleBuffered = true;
            battleOffset = 15;
            castleBonus = 1.5;
            
            BlueSoldierList = new List<Soldier>();
            RedSoldierList = new List<Soldier>();
          
            BlueCastle = new Castle(10, castleYPos, Color.Blue, castleSize, castleSize, "Blue Castle");
            RedCastle = new Castle(this.Width - 60, castleYPos, Color.Red, castleSize, castleSize, "Red Castle");            

            lbBlueGold.Text = BlueCastle.GetGold().ToString();
            lbRedGold.Text = RedCastle.GetGold().ToString();
            lbRedHp.Text = RedCastle.GetHp().ToString();
            lbBlueHP.Text = BlueCastle.GetHp().ToString();
        }

        private void DrawCastle(Graphics graphics)
        {
            Graphics gObject = graphics;
            SolidBrush drawBrush = new SolidBrush(BlueCastle.GetColor());
            Pen drawPen = new Pen(BlueCastle.GetColor());
            
            gObject.FillRectangle(drawBrush, BlueCastle.GetXPos(), BlueCastle.GetYPos(), BlueCastle.GetWidth(), BlueCastle.GetHeight());

            drawBrush = new SolidBrush(RedCastle.GetColor());
            drawPen = new Pen(RedCastle.GetColor());

            gObject.FillRectangle(drawBrush, RedCastle.GetXPos(), RedCastle.GetYPos(), RedCastle.GetWidth(), RedCastle.GetHeight());
        }

        private void DrawSoldiers(Graphics graphics, List<Soldier> soldierList, int speedBuff)
        {
            Graphics gObject = graphics;
            SolidBrush drawBrush;
            Pen drawPen;
                            
            foreach (Soldier soldier in soldierList)
            {
                soldier.SetTempSpeedBuff(0);                
            }

            foreach( Soldier soldier in soldierList)
            {                
                drawBrush = new SolidBrush(soldier.GetColor());
                drawPen = new Pen(soldier.GetColor());
                gObject.FillEllipse(drawBrush, soldier.GetXPos(), soldier.GetYPos(), soldier.GetWidth(), soldier.GetHeight());
            }
        }

        private Thread StartMovement(Soldier soldier)
        {
            Thread moveThread = new Thread(new ThreadStart(soldier.Move));
            moveThread.IsBackground = true;
            moveThread.Start();

            return moveThread;
        }

        private Soldier CreateSoldier(List<Soldier> soldierList, Castle castle, Label lbTroops, float xOffset, int xSpeed, Color color, int path)
        {
            Soldier soldier = new Soldier(castle.GetXPos() + xOffset, castle.GetYPos() + 25, color, 5, 5, SoldierTypes.Soldier.ToString(), castle.GetSoldierCost(), 3, 1, 3, 1, 2, xSpeed, path, this);
            soldier.MoveThread = StartMovement(soldier);
            soldierList.Add(soldier);
            castle.IncrementTroopCount();
            lbTroops.Text = castle.GetTroopCount().ToString();

            return (soldier);
        }

        private Soldier CreateSergeant(List<Soldier> soldierList, Castle castle, Label lbTroops, float xOffset, int xSpeed, Color color, int path)
        {
            Soldier soldier = new Soldier(castle.GetXPos() + xOffset, castle.GetYPos() + 25, color, 7, 7, SoldierTypes.Sergeant.ToString(), castle.GetSergeantCost(), 5, 2, 4, 2, 2, xSpeed, path, this);
            soldier.MoveThread = StartMovement(soldier);
            soldierList.Add(soldier);
            castle.IncrementTroopCount();
            lbTroops.Text = castle.GetTroopCount().ToString();

            return (soldier);
        }

        private Soldier CreateCavalry(List<Soldier> soldierList, Castle castle, Label lbTroops, float xOffset, int xSpeed, Color color, int path)
        {
            Soldier soldier = new Soldier(castle.GetXPos() + xOffset, castle.GetYPos() + 25, color, 11, 11, SoldierTypes.Cavalry.ToString(), castle.GetCavalryCost(), 7, 3, 7, 3, 3, xSpeed, path, this);
            soldier.MoveThread = StartMovement(soldier);
            soldierList.Add(soldier);
            castle.IncrementTroopCount();
            lbTroops.Text = castle.GetTroopCount().ToString();

            return (soldier);
        }

        private Soldier CreateCaptain(List<Soldier> soldierList, Castle castle, Label lbTroops, float xOffset, int xSpeed, Color color, int path)
        {
            Soldier soldier = new Soldier(castle.GetXPos() + xOffset, castle.GetYPos() + 25, color, 9, 9, SoldierTypes.Captain.ToString(), castle.GetCaptainCost(), 6, 5, 8, 4, 3, xSpeed, path, this);
            soldier.MoveThread = StartMovement(soldier);
            soldierList.Add(soldier);
            castle.IncrementTroopCount();
            lbTroops.Text = castle.GetTroopCount().ToString();

            return (soldier);
        }

        private bool DetermineCollision(BaseObject attacker, BaseObject defender, float offset)
        {
            if (offset == 0)
            {
                if (attacker.GetXPos() >= defender.GetXPos() && attacker.GetXPos() <= (defender.GetXPos() + defender.GetWidth()) && attacker.GetYPos() >= defender.GetYPos() && attacker.GetYPos() <= (defender.GetYPos() + defender.GetHeight()))
                {
                    return (true);
                }              
            }
            else if( Math.Abs(attacker.GetXPos() - defender.GetXPos()) <= offset && attacker.GetYPos() == defender.GetYPos())
            {
                return (true);
            }

            return (false);
        }

        private bool Battle(Soldier soldier, Soldier enemySoldier)
        {
            Random random = new Random((int)soldier.GetXPos() + (int)soldier.GetYPos() + (int)enemySoldier.GetYPos() + (int)enemySoldier.GetYPos() + DateTime.Now.Second);
            int number;

            while (true)
            {
                number = random.Next(0, 2);

                if (number == 0)
                {
                    int damage = soldier.GetAttack() - enemySoldier.GetDefense();

                    if (damage <= 0)
                    {
                        damage = 1;
                    }

                    enemySoldier.SetHp(enemySoldier.GetHP() - damage);

                    if (enemySoldier.GetHP() <= 0)
                    {
                        return (true);
                    }

                    //enemy attacks

                    damage = enemySoldier.GetAttack() - soldier.GetDefense();

                    if (damage <= 0)
                    {
                        damage = 1;
                    }

                    soldier.SetHp(soldier.GetHP() - damage);

                    if (soldier.GetHP() <= 0)
                    {
                        return (false);
                    }
                }
                else
                {
                    int damage = enemySoldier.GetAttack() - soldier.GetDefense();

                    if (damage <= 0)
                    {
                        damage = 1;
                    }

                    soldier.SetHp(soldier.GetHP() - damage);

                    if (soldier.GetHP() <= 0)
                    {
                        return (false);
                    }

                    //soldier attacks

                    damage = soldier.GetAttack() - enemySoldier.GetDefense();

                    if (damage <= 0)
                    {
                        damage = 1;
                    }

                    enemySoldier.SetHp(enemySoldier.GetHP() - damage);

                    if (enemySoldier.GetHP() <= 0)
                    {
                        return (true);
                    }
                }
            }
        }

        private void CheckPosition(List<Soldier> soldierList, List<Soldier> enemySoldierList, Castle castle, Label lbTroops, Label lbEnemyTroops, Label hp, Castle enemyCastle, float border, float secondBorder, Label kills, Label enemyKills)
        {
            List<Soldier> soldierIndexToRemove = new List<Soldier>();
            List<Soldier> enemySoldierIndexToRemove = new List<Soldier>();
         
            for (int soldierIndex = 0; soldierIndex < soldierList.Count; soldierIndex++)
            {
                for (int enemySoldierIndex = 0; enemySoldierIndex < enemySoldierList.Count; enemySoldierIndex++)
                {
                    if (DetermineCollision(soldierList[soldierIndex], enemySoldierList[enemySoldierIndex], battleOffset))
                    {
                        if (Battle(soldierList[soldierIndex], enemySoldierList[enemySoldierIndex]))
                        {
                            tbEvents.AppendText(DateTime.Now.TimeOfDay.ToString() + ": A " + castle.GetName() + " " + soldierList[soldierIndex].GetName() + " has killed a " + enemyCastle.GetName() + "'s " + enemySoldierList[enemySoldierIndex].GetName() + "." + Environment.NewLine);
                            tbEvents.ScrollToCaret();
                            castle.SetGold((int)(enemySoldierList[enemySoldierIndex].GetCost() * castleBonus));
                            enemyCastle.SetGold((int)(enemySoldierList[enemySoldierIndex].GetCost() * .25));

                            enemySoldierIndexToRemove.Add(enemySoldierList[enemySoldierIndex]);
                            enemyCastle.DecrementTroopCount();

                            kills.Text = (Int16.Parse(kills.Text) + 1).ToString();
                            lbEnemyTroops.Text = enemyCastle.GetTroopCount().ToString();

                            soldierIndex = soldierList.Count;
                            enemySoldierIndex = enemySoldierList.Count;
                            break;
                        }
                        else
                        {
                            tbEvents.AppendText(DateTime.Now.TimeOfDay.ToString() + ": A " + enemyCastle.GetName() + " " + enemySoldierList[enemySoldierIndex].GetName() + " has killed a " + castle.GetName() + "'s " + soldierList[soldierIndex].GetName() + "." + Environment.NewLine);
                            tbEvents.ScrollToCaret();
                            enemyCastle.SetGold((int)(soldierList[soldierIndex].GetCost() * castleBonus));
                            castle.SetGold((int)(soldierList[soldierIndex].GetCost() * .25));

                            soldierIndexToRemove.Add(soldierList[soldierIndex]);
                            castle.DecrementTroopCount();

                            enemyKills.Text = (Int16.Parse(enemyKills.Text) + 1).ToString();
                            lbTroops.Text = castle.GetTroopCount().ToString();

                            soldierIndex = soldierList.Count;
                            enemySoldierIndex = enemySoldierList.Count;
                            break;
                        }
                    }
                }                    
            }

            if (soldierIndexToRemove.Count > 0 || enemySoldierIndexToRemove.Count > 0)
            {
                RemoveSoldiers(soldierIndexToRemove, enemySoldierIndexToRemove, soldierList, enemySoldierList);

                this.Refresh();
            }

            for (int soldierIndex = 0; soldierIndex < soldierList.Count; soldierIndex++)
            {

                if (DetermineCollision(soldierList[soldierIndex], enemyCastle, 0))
                {
                    tbEvents.AppendText(DateTime.Now.TimeOfDay.ToString() + ": " + castle.GetName() + "'s " + soldierList[soldierIndex].GetName() + " has attacked " + enemyCastle.GetName() + "." + Environment.NewLine);
                    tbEvents.ScrollToCaret();
                    enemyCastle.DecrementHP(soldierList[soldierIndex].GetCastleDamage());
                    castle.SetGold((int)(soldierList[soldierIndex].GetCost() * .75));
                    enemyCastle.SetGold((int)(soldierList[soldierIndex].GetCost() * .25));

                    soldierIndexToRemove.Add(soldierList[soldierIndex]);
                    castle.DecrementTroopCount();

                    lbTroops.Text = castle.GetTroopCount().ToString();
                    hp.Text = enemyCastle.GetHp().ToString();

                    break;
                }
                else if (soldierList[soldierIndex].GetXPos() >= this.Width || soldierList[soldierIndex].GetXPos() <= 0)
                {                   
                    castle.SetGold(soldierList[soldierIndex].GetCost() / 2);

                    soldierIndexToRemove.Add(soldierList[soldierIndex]);
                    castle.DecrementTroopCount();

                    lbTroops.Text = castle.GetTroopCount().ToString();   
                    
                    break;
                }
            }

            if (soldierIndexToRemove.Count > 0 || enemySoldierIndexToRemove.Count > 0)
            {
                RemoveSoldiers(soldierIndexToRemove, enemySoldierIndexToRemove, soldierList, enemySoldierList);

                this.Refresh();
            }

          
        }

        private void RemoveSoldiers(List<Soldier> p_lstSoldierIndexToRemove, List<Soldier> p_lstEnemySoldierIndexToRemove, List<Soldier> p_lstSoldierList, List<Soldier> p_lstEnemySoldierList)
        {
            foreach(Soldier index in p_lstSoldierIndexToRemove)
            {
                p_lstSoldierList.Find(x => x == index).MoveThread.Abort();
                p_lstSoldierList.RemoveAll(x => x == index);
            }

            p_lstSoldierIndexToRemove.Clear();

            foreach (Soldier index in p_lstEnemySoldierIndexToRemove)
            {
                p_lstEnemySoldierList.Find(x => x == index).MoveThread.Abort();
                p_lstEnemySoldierList.RemoveAll(x => x == index);
            }

            p_lstEnemySoldierIndexToRemove.Clear();          
        }

        private int DeterminePath(Castle castle, Castle enemyCastle, List<Soldier> soldierList, List<Soldier> enemySoldierList)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            int number = random.Next(0, 5);

            //there will be code to make a smarter decision as to where the soldier should spawn

            return (number);
        }

        private void PerformActions()
        {
            int actions;
            PurchaseAction purchase;                       

            BlueCastle.SetGold(BlueCastle.GetGoldPerTick());
            RedCastle.SetGold(RedCastle.GetGoldPerTick());

            if (tickCount % tickModifier == 0)
            {
                actions = BlueCastle.GetActionsPerTick();

                while (actions > 0)
                {
                    purchase = BlueCastle.DetermineBuy(tickCount + actions);

                    int path = DeterminePath(BlueCastle, RedCastle, BlueSoldierList, RedSoldierList);

                    DetermineAction(purchase, BlueCastle, BlueSoldierList, lbBlueTroops, 50, 1, Color.Blue, path);

                    actions--;
                }

                IncreaseTroopAttack(BlueCastle, BlueSoldierList);
                IncreaseTroopDefense(BlueCastle, BlueSoldierList);

                actions = RedCastle.GetActionsPerTick();

                while (actions > 0)
                {
                    purchase = RedCastle.DetermineBuy(tickCount + actions);

                    int path = DeterminePath(RedCastle, BlueCastle, RedSoldierList, BlueSoldierList);

                    DetermineAction(purchase, RedCastle, RedSoldierList, lbRedTroops, 0, -1, Color.Red, path);

                    actions--;                
                }

                IncreaseTroopAttack(RedCastle, RedSoldierList);
                IncreaseTroopDefense(RedCastle, RedSoldierList);
            }            
            
            lbBlueGold.Text = BlueCastle.GetGold().ToString();
            lbRedGold.Text = RedCastle.GetGold().ToString();
            tickCount++;
        }

        private void DetermineAction(PurchaseAction purchase, Castle castle, List<Soldier> soldierList, Label lbTroops, float xOffset, int xSpeed, Color color, int path)
        {
            if (purchase == PurchaseAction.Soldier)
            {
                CreateSoldier(soldierList, castle, lbTroops, xOffset, xSpeed, color, path);

                tbEvents.AppendText("A Soldier was created for " + castle.GetName() + "." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.Sergeant)
            {
                CreateSergeant(soldierList, castle, lbTroops, xOffset, xSpeed, color, path);

                tbEvents.AppendText("A Sergeant was created for " + castle.GetName() + "." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.Cavalry)
            {
                CreateCavalry(soldierList, castle, lbTroops, xOffset, xSpeed, color, path);

                tbEvents.AppendText("A Cavalry was created for " + castle.GetName() + "." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.Captain)
            {
                CreateCaptain(soldierList, castle, lbTroops, xOffset, xSpeed, color, path);

                tbEvents.AppendText("A Captain was created for " + castle.GetName() + "." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchTools)
            {
                tbEvents.AppendText(castle.GetName() + " has researched Tools.  Actions have increased." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchFarming)
            {
                tbEvents.AppendText(castle.GetName() + " has researched Farms.  Actions have increased.  Troop limit has increased to 10." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchRoads)
            {
                tbEvents.AppendText(castle.GetName() + " has researched Roads.  Actions have increased." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchEconomy)
            {
                castle.AddGoldPerTick(5);

                tbEvents.AppendText(castle.GetName() + " now produces more gold.  Actions have increased." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchEfficiency)
            {
                tbEvents.AppendText(castle.GetName() + " has researched Efficiency.  Actions have increased." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchTraining)
            {
                tbEvents.AppendText(castle.GetName() + " has researched Training.  Actions have increased." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchBronzeWeapons)
            {
                castle.IncreaseTroopAttack(1);
                castle.IncrementUnitCosts(35, 200, 600, 350);                

                tbEvents.AppendText("Bronze Weapons have been researched for " + castle.GetName() + ".  Attack power has increased." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchIronWeapons)
            {
                castle.IncreaseTroopAttack(2);
                castle.IncrementUnitCosts(70, 400, 1200, 700);

                tbEvents.AppendText("Iron Weapons have been researched for " + castle.GetName() + ".  Attack power has increased." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchSteelWeapons)
            {
                castle.IncreaseTroopAttack(4);
                castle.IncrementUnitCosts(140, 800, 2400, 1400);
                              
                tbEvents.AppendText("Steel Weapons have been researched for " + castle.GetName() + ".  Attack power has increased." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchBronzeArmor)
            {
                castle.IncreaseTroopDefense(1);
                castle.IncrementUnitCosts(35, 200, 600, 350);
                                
                tbEvents.AppendText("Bronze Armor has been researched for " + castle.GetName() + ".  Defense has increased." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchIronArmor)
            {
                castle.IncreaseTroopDefense(2);
                castle.IncrementUnitCosts(70, 400, 1200, 700);

                tbEvents.AppendText("Iron Armor has been researched for " + castle.GetName() + ".  Defense has increased." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchSteelArmor)
            {
                castle.IncreaseTroopDefense(3);
                castle.IncrementUnitCosts(140, 800, 2400, 1400);

                tbEvents.AppendText("Steel Armor has been researched for " + castle.GetName() + ".  Defense has increased." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchSergeant)
            {
                tbEvents.AppendText(castle.GetName() + " can now build Sergeants." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchCaptain)
            {
                tbEvents.AppendText(castle.GetName() + " can now build Captains." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchCavalry)
            {
                tbEvents.AppendText(castle.GetName() + " can now build Cavalry." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchTower)
            {
                tbEvents.AppendText(castle.GetName() + " can now build Towers." + Environment.NewLine);
                tbEvents.ScrollToCaret();
            }
            else if (purchase == PurchaseAction.ResearchBasicMining)
            {
                tbEvents.AppendText(castle.GetName() + " has researched Basic Mining.  Gold production increased." + Environment.NewLine);
                tbEvents.ScrollToCaret();

                castle.AddGoldPerTick(5);
            }
            else if (purchase == PurchaseAction.ResearchGoldPanning)
            {
                tbEvents.AppendText(castle.GetName() + " has researched Gold Panning.  Gold production increased." + Environment.NewLine);
                tbEvents.ScrollToCaret();

                castle.AddGoldPerTick(5);
            }
            else if (purchase == PurchaseAction.ResearchStripMining)
            {
                tbEvents.AppendText(castle.GetName() + " has researched Strip Mining.  Gold production increased." + Environment.NewLine);
                tbEvents.ScrollToCaret();

                castle.AddGoldPerTick(5);
            }
            else if (purchase == PurchaseAction.ResearchAdvancedMining)
            {
                tbEvents.AppendText(castle.GetName() + " has researched Advanced Mining.  Gold production increased." + Environment.NewLine);
                tbEvents.ScrollToCaret();

                castle.AddGoldPerTick(5);
            }           
        }

        private void IncreaseTroopDefense(Castle castle, List<Soldier> soldierList)
        {
            foreach (Soldier soldier in soldierList)
            {
                if (soldier.GetName() == SoldierTypes.Soldier.ToString())
                {
                    soldier.SetTroopDefense(castle.GetBonusDefense());                    
                }
                else if (soldier.GetName() == SoldierTypes.Sergeant.ToString())
                {
                    soldier.SetTroopDefense((int)(castle.GetBonusDefense() * 1.5));
                }
                else if (soldier.GetName() == SoldierTypes.Captain.ToString())
                {
                    soldier.SetTroopDefense(castle.GetBonusDefense() * 2);
                }
                else if (soldier.GetName() == SoldierTypes.Cavalry.ToString())
                {
                    soldier.SetTroopDefense((int)(castle.GetBonusDefense() * 2.5));
                }
                else if (soldier.GetName() == SoldierTypes.Tower.ToString())
                {
                    soldier.SetTroopDefense(castle.GetBonusDefense() * 3);
                }
            }
        }

        private void IncreaseTroopAttack(Castle castle, List<Soldier> soldierList)
        {
            foreach (Soldier soldier in soldierList)
            {
                if (soldier.GetName() == SoldierTypes.Soldier.ToString())
                {
                    soldier.SetTroopAttack(castle.GetBonusAttack());                    
                }
                else if (soldier.GetName() == SoldierTypes.Sergeant.ToString())
                {
                    soldier.SetTroopAttack((int)(castle.GetBonusAttack() * 1.5));
                }
                else if (soldier.GetName() == SoldierTypes.Captain.ToString())
                {
                    soldier.SetTroopAttack(castle.GetBonusAttack() * 2);
                }
                else if (soldier.GetName() == SoldierTypes.Cavalry.ToString())
                {
                    soldier.SetTroopAttack((int)(castle.GetBonusAttack() * 2.5));
                }
                else if (soldier.GetName() == SoldierTypes.Tower.ToString())
                {
                    soldier.SetTroopAttack(castle.GetBonusAttack() * 3);
                }
            }
        }

        private void StopThreads()
        {
            timer1.Stop();

            foreach (Soldier soldier in BlueSoldierList)
            {
                soldier.MoveThread.Abort();               
            }

            foreach (Soldier soldier in RedSoldierList)
            {
                soldier.MoveThread.Abort();                
            }           

            Application.Exit();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawCastle(e.Graphics);
                        
            DrawSoldiers(e.Graphics, BlueSoldierList, 2);
            CheckPosition(BlueSoldierList, RedSoldierList, BlueCastle, lbBlueTroops, lbRedTroops, lbRedHp, RedCastle, this.Width - 20, this.Width, lbBlueKills, lbRedKills);
                                   
            DrawSoldiers(e.Graphics, RedSoldierList, -2);
            CheckPosition(RedSoldierList, BlueSoldierList, RedCastle, lbRedTroops, lbBlueTroops, lbBlueHP, BlueCastle, 1, 20, lbRedKills, lbBlueKills);
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopThreads();            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            PerformActions();
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }      
    }
}