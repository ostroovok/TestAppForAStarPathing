namespace Cells
{

    public static class PathingConstants
    {
        public static readonly StepDirection[] Directions = {
            new StepDirection(-1, +0), // W     left
            new StepDirection(+1, +0), // E     right
            new StepDirection(+0, -1), // N     top
            new StepDirection(+0, +1), // S     bottom
        };
    }

}