using GeometryFriends;
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
        public float h;
        private Boolean vel_dependable;


        public State(float velX, float velY, float posX, float posY, float H)
        {
            this._velocityX = velX;
            this._velocityY = velY;
            this._posX = posX;
            this._posY = posY + H ;
            h = H;
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
        public void updateState(float X, float Y, float VelocityX, float VelocityY, float h)
        {
            this._posX = X;
             this._posY = Y ;
            this._velocityX = VelocityX;
            this._velocityY = VelocityY;
        }
        public void updateHeight()
        {
            this._posY += (720 - this._posY)/2 ;
            Log.LogInformation("New height" + this._posY.ToString());
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
