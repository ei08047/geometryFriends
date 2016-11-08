using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents
{
    public class State
    {
        private float _velocityX;
        private float _velocityY;
        private float _posX;
        private float _posY;

        private Boolean vel_dependable;
        private float radius= 20;

        public State(float velX, float velY, float posX, float posY)
        {
            this._velocityX = velX;
            this._velocityY = velY;
            this._posX = posX;
            this._posY = posY;
            vel_dependable = true;
        }

        public State(float posX, float posY)
        {
            this._posX = posX;
            this._posY = posY;
            vel_dependable = false;
        }

        public void setRadius(float r)
        {
            radius = r;
        }

        public State getState()
        {
            return this;
        }


        public float getX()
        { return this._posX;  }

        public float getY()
        { return this._posY; }




    }
}
