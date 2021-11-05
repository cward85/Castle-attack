using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CastleAttack
{
    public class BaseObject
    {
        protected float xPos, yPos;
        protected Color color;
        protected float width, height;
        protected string name;

        public float GetXPos()
        {
            return (xPos);
        }

        public float GetYPos()
        {
            return (yPos);
        }

        public float GetWidth()
        {
            return (width);
        }

        public float GetHeight()
        {
            return (height);
        }

        public Color GetColor()
        {
            return (color);
        }

        public string GetName()
        {
            return (name);
        }

        public void SetXPos(float x)
        {
            xPos += x;
        }

        public void SetYPos(float y)
        {
            yPos += y;
        }
    }
}
