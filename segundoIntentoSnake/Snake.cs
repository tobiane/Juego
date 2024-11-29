﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace segundoIntentoSnake
{
    internal class Snake
    {
        Texture2D snakeSheet;
        Vector2 snakePosition;
        char snakeDirection = 'R';
        float snakeSpeed;
        List<Part> bodyParts;
        Vector2 applePosition;
        Rectangle apple = new Rectangle(0, 192, 64, 64);

        public Texture2D SnakeSheet { get { return snakeSheet; } set { snakeSheet = value; } }
        public Vector2 SnakePosition { get { return snakePosition; } set { snakePosition = value; } }
        public char SnakeDirection { get { return snakeDirection; } set { snakeDirection = value; } }
        public float SnakeSpeed { get { return snakeSpeed; } set { snakeSpeed = value; } }
        public Vector2 ApplePosition { get { return applePosition; } set { applePosition = value; } }


        public Snake(List<Part> bodyParts)
        {
            this.bodyParts = bodyParts;
        }

        public void UpdateBody() 
        {
            for (int i = bodyParts.Count - 1; i > 0; i--)
            {
                bodyParts[i].Position = bodyParts[i - 1].Position;
                bodyParts[i].Direction = bodyParts[i - 1].Direction;
            }

            bodyParts[0].Position = snakePosition;
            bodyParts[0].Direction = snakeDirection;

            PartDefinition();
        }
        public void PartDefinition() 
        {
            Part lastPart = bodyParts[bodyParts.Count - 1];
            Part almostLastPart = bodyParts[bodyParts.Count - 2];

            lastPart.Direction = almostLastPart.Direction;

            for (int i = 0; i < bodyParts.Count; i++)
            {
                if (i == 0) 
                { 
                    if (bodyParts[i].Direction == 'L') 
                        bodyParts[i].Type = Part.SnakePartType.HeadHorizontalLeft;
                    else if (bodyParts[i].Direction == 'R')
                        bodyParts[i].Type = Part.SnakePartType.HeadHorizontalRight;
                    else if (bodyParts[i].Direction == 'U')
                        bodyParts[i].Type = Part.SnakePartType.HeadVerticalUp;
                    else if (bodyParts[i].Direction == 'D')
                        bodyParts[i].Type = Part.SnakePartType.HeadVerticalDown;
                }
                else if (i == bodyParts.Count - 1) 
                { 
                    if (bodyParts[i].Direction == 'L')
                        bodyParts[i].Type = Part.SnakePartType.TailHorizontalLeft;
                    else if (bodyParts[i].Direction == 'R')
                        bodyParts[i].Type = Part.SnakePartType.TailHorizontalRight;
                    else if (bodyParts[i].Direction == 'U')
                        bodyParts[i].Type = Part.SnakePartType.TailVerticalUp;
                    else if (bodyParts[i].Direction == 'D')
                        bodyParts[i].Type = Part.SnakePartType.TailVerticalDown;
                }
                else
                {
                    if (bodyParts[i].Direction != bodyParts[i - 1].Direction)
                    {
                        if ((bodyParts[i - 1].Direction == 'U' && bodyParts[i].Direction == 'R') ||
                            (bodyParts[i - 1].Direction == 'L' && bodyParts[i].Direction == 'D'))
                            bodyParts[i].Type = Part.SnakePartType.BodyCornerBottomRight;
                        else if ((bodyParts[i - 1].Direction == 'U' && bodyParts[i].Direction == 'L') ||
                                 (bodyParts[i - 1].Direction == 'R' && bodyParts[i].Direction == 'D'))
                            bodyParts[i].Type = Part.SnakePartType.BodyCornerBottomLeft;
                        else if ((bodyParts[i - 1].Direction == 'D' && bodyParts[i].Direction == 'R') ||
                                 (bodyParts[i - 1].Direction == 'L' && bodyParts[i].Direction == 'U'))
                            bodyParts[i].Type = Part.SnakePartType.BodyCornerTopRight;
                        else if ((bodyParts[i - 1].Direction == 'D' && bodyParts[i].Direction == 'L') ||
                                 (bodyParts[i - 1].Direction == 'R' && bodyParts[i].Direction == 'U'))
                            bodyParts[i].Type = Part.SnakePartType.BodyCornerTopLeft;
                    }
                    else if (bodyParts[i].Direction == 'L' || bodyParts[i].Direction == 'R')
                    {
                        bodyParts[i].Type = Part.SnakePartType.BodyHorizontal;
                    }
                    else
                    {
                        bodyParts[i].Type = Part.SnakePartType.BodyVertical;
                    }
                }
            }
        }
        public bool GameOver(List<Part> bodyParts2)
        {
            for (int i = 1; i < bodyParts.Count; i++)
            {
                if (bodyParts[i].Position == bodyParts[0].Position)    
                {
                    snakeDirection = 'T';
                    return true;
                }
            }

            for (int i = 0; i < bodyParts2.Count; i++)
            {
                if (bodyParts[0].Position == bodyParts2[i].Position)
                {
                    snakeDirection = 'T';
                    return true;
                }
            }

            for (int i = 1; i < bodyParts2.Count; i++)
            {
                if (bodyParts2[i].Position == bodyParts2[0].Position)
                {
                    snakeDirection = 'T';
                    return true;
                }
            }

            for (int i = 0; i < bodyParts.Count; i++)
            {
                if (bodyParts2[0].Position == bodyParts[i].Position)
                {
                    snakeDirection = 'T';
                    return true;
                }
            }

            return false;
        }
        public void GenerateApplePosition(Random random, GraphicsDeviceManager graphics, List<Part> bodyParts2, Vector2 applePosition2)
        {
            int cellSize = 32;

            int gridWidth = graphics.PreferredBackBufferWidth / cellSize;
            int gridHeight = graphics.PreferredBackBufferHeight / cellSize;
            //random.Next(0, _graphics.PreferredBackBufferWidth / cellSize) * cellSize, random.Next(0, _graphics.PreferredBackBufferHeight / cellSize) * cellSize
            applePosition = new Vector2(random.Next(0, gridWidth) * cellSize, random.Next(0, gridHeight) * cellSize);

            foreach(Part i in bodyParts)
            {
                if (applePosition == i.Position)
                    GenerateApplePosition(random, graphics, bodyParts2, applePosition2);
            }
            foreach(Part i in bodyParts2)
            {
                if (applePosition == i.Position)
                    GenerateApplePosition(random, graphics, bodyParts2, applePosition2);
            }
            if(applePosition == applePosition2)
                GenerateApplePosition(random, graphics, bodyParts2, applePosition2);
        }
        public void DrawApple(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
            snakeSheet,
            applePosition,
            apple,
            Color.White,
            0f,
            Vector2.Zero,
            new Vector2(0.5f, 0.5f),
            SpriteEffects.None,
            0f);
        }

        public void DrawSnake(SpriteBatch spriteBatch, Color snakeColor)
        {
            for (int i = 0; i < bodyParts.Count; i++)
            {
                spriteBatch.Draw(
                snakeSheet,
                bodyParts[i].Position,
                bodyParts[i].RectanglePart(),
                snakeColor,
                0f,
                Vector2.Zero,
                new Vector2(0.5f, 0.5f),
                SpriteEffects.None,
                0f);
            }
        }
    }
}
