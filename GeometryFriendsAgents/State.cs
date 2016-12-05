using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents
{
    public class State
    {
        public float _velocityX;
        public float _velocityY;
        public float _posX;
        public float _posY;
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
        public State getState()
        {
            return this;
        }
        public void updateState(float X, float Y, float VelocityX, float VelocityY)
        {
            this._posX = X;
            this._posY = Y;
            this._velocityX = VelocityX;
            this._velocityY = VelocityY;
        }
        public float getX()
        { return this._posX;  }
        public float getY()
        { return this._posY; }
        public float getVx()
        {
            return _velocityX;
        }
        public float getVy()
        {
            return _velocityY;
        }
        public void setVx(float v)
        {
            _velocityX = v;
        }
        public void setVy(float v) {
            _velocityY = v;
        }
        public void setRadius(float r)
        {
            radius = r;
        }
        public Boolean verticalMovement() {
            if (Math.Abs(_velocityY) > 0)
                return true;
            else
                return false;
        }
        public Boolean horizontalMovement()
        {
            if (Math.Abs(_velocityX) > 0)
                return true;
            else
                return false;
        }




    }
}
