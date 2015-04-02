using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace robotInLabyrinth
{
    /// <summary>
    /// Класс реализующий оценочную функцию
    /// </summary>
    class FunctionRating
    {
        public double functionRating(int width, int height, Point positionRobot, Point finish)
        {
            double rating = 0;
            int var;
            int varS = 0;
            int varF = 0;
            if ((positionRobot.X < width / 2) && (positionRobot.Y < height / 2))
            {
                varS = 1;
            }
            else
                if ((positionRobot.X >= width / 2) && (positionRobot.Y < height / 2))
                { varS = 2; }
                else
                    if ((positionRobot.X >= width / 2) && (positionRobot.Y >= height / 2))
                    { varS = 3; }
                    else
                        if ((positionRobot.X < width / 2) && (positionRobot.Y >= height / 2))
                        { varS = 4; }

            if ((finish.X < width / 2) && (finish.Y < height / 2))
            {
                varF = 1;
            }
            else
                if ((finish.X >= width / 2) && (finish.Y < height / 2))
                { varF = 2; }
                else
                    if ((finish.X >= width / 2) && (finish.Y >= height / 2))
                    { varF = 3; }
                    else
                        if ((finish.X < width / 2) && (finish.Y >= height / 2))
                        { varF = 4; }
            var = varS * 4 - 4 + varF;
            switch (var)
            {
                //робот в 1 четверти выход в 2.
                case 2:
                //робот в 1 четверти выход в 3.
                case 3:
                //робот в 1 четверти выход в 4.Движение вниз и вправо.Причем приоритетнее движение вниз. 
                case 4: rating = positionRobot.X * 1.01 / (width - 1) + (double)positionRobot.Y / (double)(height - 1); break;

                //робот в 3 четверти выход в 2.
                case 10:
                    if ((positionRobot.X != width - 1) && (positionRobot.Y != height - 1))
                    {
                        rating = positionRobot.X * 1.01 / (width - 1) + (double)positionRobot.Y / (double)(height - 1);
                    }
                    //При достижении правой крайней стены движение вверх.
                    else if (positionRobot.X == (width - 1))
                    {
                        rating = 1.01 + 1 + 1 + 1 - (double)positionRobot.Y / (double)height;
                    }
                    //При достижении крайней нижней стены движение вправо 
                    else if (positionRobot.Y == (height - 1))
                    {
                        rating = 1.01 + 1 + (double)positionRobot.X / (double)height;
                    }
                    break;

                //робот в 2 четверти выход в 3.Движение вниз и вправо.
                case 7:
                //робот во 2 четверти выход в 4.Движение вниз и вправо.     
                case 8:
                //робот в 3 четверти выход в 4.Движение вниз и вправо. 
                case 12:
                    if ((positionRobot.X != width - 1) && (positionRobot.Y != height - 1))
                    {
                        rating = positionRobot.X * 1.01 / (width - 1) + (double)positionRobot.Y / (double)(height - 1);
                    }
                    //При достижении правой крайней стены движение вниз.
                    else if (positionRobot.X == (width - 1))
                    {
                        rating = 1.01 + (double)positionRobot.Y / (double)height;
                    }
                    //При достижении крайней нижней стены движение влево 
                    else if (positionRobot.Y == (height - 1))
                    {
                        rating = 1.01 + 1 + 1 - (double)positionRobot.X / (double)height;
                    }
                    break;
                //робот в 4 четверти выход в 2.Движение вниз и вправо.
                case 14:
                    if ((positionRobot.X != width - 1) && (positionRobot.Y != height - 1))
                    {
                        rating = positionRobot.X * 1.01 / (width - 1) + (double)positionRobot.Y / (double)(height - 1);
                    }
                    break;
                //робот в 4 четверти выход в 3.Движение вниз и вправо.
                case 15:

                    if (positionRobot.Y != height - 1)
                    {
                        rating = positionRobot.X * 1.01 / (width - 1) + (double)positionRobot.Y / (double)(height - 1);
                    }
                    //При достижении крайней нижней стены движение вправо 
                    else
                    {
                        rating = 1.01 + 1 + (double)positionRobot.X / (double)height;
                    }
                    break;
                //робот в 1 четверти выход в 1.Движение к выходу.
                case 1:
                //робот в 2 четверти выход в 2.Движение к выходу.
                case 6:
                //робот в 3 четверти выход в 3.Движение к выходу.
                case 11:
                //робот в 4 четверти выход в 4.Движение к выходу. 
                case 16: rating = 1.01 + 2 + 1 - (double)Math.Abs(positionRobot.X - finish.X) / (double)(width - 1) / 2 + 1
                     - (double)Math.Abs(positionRobot.Y - finish.Y) / (double)(height - 1) / 2;
                    break;

            }
            return rating;
        }
    }
}
