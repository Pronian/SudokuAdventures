using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuAdv.Data
{
    static class PuzzleIDs
    {

        public static int[] CampaingPuzzles = { 319, 134, 256, 193, 405, 308, 2 , 462, 850,  //Beginner
                                                706, 6, 477, 110, 280, 252, 709, 414, 1028,  //Easy
                                                2128, 2490, 2507, 3503, 3509, 3537, 3620, 3728, 3763, //Medium
                                                3773, 3799, 3811, 3812, 2461, 3496, 3514, 3768, 3782,
                                                3785, 3839, 2470, 3541, 3808, 3826, 3712, 3742, 3761,
                                                2466, 3798, 3872, 3881, 3880, 3865, 3790, 3869, 3860, //hard
                                                3879, 3878, 3870, 3784, 3858, 3890, 3897, 3916, 3894,
                                                3883, 3506, 3899, 3893, 3887, 3831, 3882, 3817, 3910,
                                                3895, 3931, 3932, 3933, 3930, 3929, 3889, 3892, 3859};//Very Hard

        //Clues 50-38
        public static int[] BeginnerPuzzles = { 257, 319, 871, 884, 867, 873, 931 };
        //Clues 36-32
        public static int[] EasyPuzzles = { 574 , 973};
        //Clues 31-28
        public static int[] MediumPuzzles = { 2458, 3589, 3816, 1220, 583, 682, 3605, 3553, 3538 };
        //Clues 27-diff 100
        public static int[] HardPuzzles = { 3742,3712};
        //Clues Diff 155-1500
        public static int[] VeryHardPuzzles = { 2219, 2038, 1800, 1741, 3776, 3657, 3745, 1176, 126, 268, 
                                               3532, 1427, 3675, 3775, 3779 };

        public static int[][] All = { CampaingPuzzles, BeginnerPuzzles, EasyPuzzles, MediumPuzzles, HardPuzzles, VeryHardPuzzles };
    }
}
