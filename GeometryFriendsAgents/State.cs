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

        public State(float velX, float velY, float posX, float posY)
        {
            this._velocityX = velX;
            this._velocityY = velY;
            this._posX = posX;
            this._posY = posY;
        }
    }
}
