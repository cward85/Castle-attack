using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Threading;

namespace CastleAttack
{
    class Soldier : BaseObject
    {    
        public delegate void DelegateCallback();

        public enum SoldierTypes
        {
            Soldier = 1,
            Sergeant = 2,
            Captain = 3,
            Cavalry = 4,
            Tower = 5
        }

        int ID;
        int xSpeed, ySpeed, cost, tempSpeedBuff;
        int hp, maxHp, minAttack, maxAttack, defense, castleDamage;
        float xLimit, yLimit;
        int path;
        int bonusAttack, bonusDefense;
        CastleAttack form;
        
        public Soldier(float x, float y, Color clr, float h, float w, string nam, int val, int maxHits, int minAtt, int maxAtt, int def, int dam, int speed, int pth, CastleAttack frm)
        {
            xPos = x;
            yPos = y;
            color = clr;
            height = h;
            width = w;
            name = nam;
            hp = maxHits;
            maxHp = maxHits;
            minAttack = minAtt;
            maxAttack = maxAtt;
            defense = def;
            castleDamage = dam;
            bonusDefense = 0;
            bonusAttack = 0;            
            form = frm;
            xLimit = form.Width;

            xSpeed = speed;            

            cost = val;
            tempSpeedBuff = 0;
            path = pth;
            ID = CastleAttack.BaseID++;
        }        

        public int GetXSpeed()
        {
            return (xSpeed);
        }

        public Thread MoveThread { get; set; }

        public int GetYSpeed()
        {
            return (ySpeed);
        }

        public int GetCost()
        {
            return (cost);
        }

        public int GetHP()
        {
            return (hp);
        }

        public int GetMaxHP()
        {
            return (maxHp);
        }

        public int GetAttack()
        {
            int attack = new Random(DateTime.Now.Millisecond).Next(minAttack, maxAttack);
            return (attack + bonusAttack);
        }

        public int GetDefense()
        {
            return (defense + bonusDefense);
        }
       
        public int GetCastleDamage()
        {
            return (castleDamage);
        }

        public int GetPath()
        {
            return (path);
        }

        public void SetHp(int val)
        {
            hp = val;
        }

        public void SetMaxHp(int val)
        {
            maxHp = val;
        }

        public void SetAttack(int val)
        {
            minAttack = val;
        }

        public void SetDefense(int val)
        {
            defense = val;
        }

        public void SetCastleDamage(int val)
        {
            castleDamage = val;
        }

        public void SetTempSpeedBuff(int val)
        {
            tempSpeedBuff = val;
        }

        public void SetTroopAttack(int amount)
        {
            bonusAttack = amount;
        }

        public void SetTroopDefense(int amount)
        {
            bonusDefense = amount;
        }

        private void RefreshForm()
        {
            if (form.InvokeRequired == true)
            {
                form.Invoke(new DelegateCallback(RefreshForm));
            }
            else
            {
                form.Validate();
            }
        }

        public void Move()
        {
            while (true)
            {                
                DetermineDirection();
                
                SetXPos(GetXSpeed() + tempSpeedBuff);
                SetYPos(GetYSpeed());

                form.Invalidate();

                Thread.Sleep(20);

                form.Validate();
            }
        }

        public void DetermineDirection()
        {
            if (color == Color.Blue)
            {
                if (path == 0)
                {
                    if (xPos <= 160)
                    {
                        ySpeed = 2 * xSpeed;
                    }
                    else if (xPos >= xLimit - 150)
                    {
                        ySpeed = -2 * xSpeed;
                    }
                    else
                    {
                        ySpeed = 0;
                    }
                }
                else if (path == 1)
                {
                    if (xPos <= 160)
                    {
                        ySpeed = xSpeed;
                    }
                    else if (xPos >= xLimit - 140)
                    {
                        ySpeed = -xSpeed;
                    }
                    else
                    {
                        ySpeed = 0;
                    }
                }
                else if (path == 2)
                {
                    ySpeed = 0;
                }
                else if (path == 3)
                {
                    if (xPos <= 160)
                    {
                        ySpeed = -xSpeed;
                    }
                    else if (xPos >= xLimit - 140)
                    {
                        ySpeed = xSpeed;
                    }
                    else
                    {
                        ySpeed = 0;
                    }
                }
                else if (path == 4)
                {
                    if (xPos <= 160)
                    {
                        ySpeed = -2 * xSpeed;
                    }
                    else if (xPos >= xLimit - 150)
                    {
                        ySpeed = 2 * xSpeed;
                    }
                    else
                    {
                        ySpeed = 0;
                    }
                }
            }
            else if (color == Color.Red)
            {
                if (path == 0)
                {
                    if (xPos >= xLimit - 160)
                    {
                        ySpeed = -2 * xSpeed;
                    }
                    else if (xPos <= 140)
                    {
                        ySpeed = 2 * xSpeed;
                    }
                    else
                    {
                        ySpeed = 0;
                    }
                }
                else if (path == 1)
                {
                    if (xPos >= xLimit - 160)
                    {
                        ySpeed = -xSpeed;
                    }
                    else if (xPos <= 140)
                    {
                        ySpeed = xSpeed;
                    }
                    else
                    {
                        ySpeed = 0;
                    }
                }
                else if (path == 2)
                {
                    ySpeed = 0;
                }
                else if (path == 3)
                {
                    if (xPos >= xLimit - 160)
                    {
                        ySpeed = xSpeed;
                    }
                    else if (xPos <= 140)
                    {
                        ySpeed = -xSpeed;
                    }
                    else
                    {
                        ySpeed = 0;
                    }
                }
                else if (path == 4)
                {
                    if (xPos >= xLimit - 160)
                    {
                        ySpeed = 2 * xSpeed;
                    }
                    else if (xPos <= 140)
                    {
                        ySpeed = -2 * xSpeed;
                    }
                    else
                    {
                        ySpeed = 0;
                    }
                }
            }
        }
    }
}
