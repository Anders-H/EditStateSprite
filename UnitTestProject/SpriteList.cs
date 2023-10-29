﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject
{
    [TestClass]
    public class SpriteList
    {
        [TestMethod]
        public void Deserialize()
        {
            const string data = @"BEGIN FILE (2023-10-24)
DOCUMENT TYPE=SPRIDEF2
DOCUMENT VERSION=1.0
BEGIN SPRITES (3)

BEGIN SPRITE (1/3)
NAME=Sprite 0 (multicolor)
MULTICOLOR=YES
PREVIEW OFFSET=30,30
EXPAND=NO
PREVIEW ZOOM=NO
COLOR PALETTE=Black-White-Red-Cyan
SPRITE ROW DATA (01/21)=200000000000
SPRITE ROW DATA (02/21)=200000000000
SPRITE ROW DATA (03/21)=200000000000
SPRITE ROW DATA (04/21)=200000000000
SPRITE ROW DATA (05/21)=200000000000
SPRITE ROW DATA (06/21)=000000000000
SPRITE ROW DATA (07/21)=000000000000
SPRITE ROW DATA (08/21)=000000000000
SPRITE ROW DATA (09/21)=000000000000
SPRITE ROW DATA (10/21)=000000000000
SPRITE ROW DATA (11/21)=000000000000
SPRITE ROW DATA (12/21)=000000000000
SPRITE ROW DATA (13/21)=000000000000
SPRITE ROW DATA (14/21)=000000000000
SPRITE ROW DATA (15/21)=000000000000
SPRITE ROW DATA (16/21)=000000000000
SPRITE ROW DATA (17/21)=000000000000
SPRITE ROW DATA (18/21)=000000000000
SPRITE ROW DATA (19/21)=000000000000
SPRITE ROW DATA (20/21)=000000000000
SPRITE ROW DATA (21/21)=000000000000
END SPRITE (1/3)

BEGIN SPRITE (2/3)
NAME=Sprite 1 (monochrome)
MULTICOLOR=NO
PREVIEW OFFSET=30,30
EXPAND=NO
PREVIEW ZOOM=NO
COLOR PALETTE=Black-White
SPRITE ROW DATA (01/21)=000000000000000000000000
SPRITE ROW DATA (02/21)=000000000000000000000000
SPRITE ROW DATA (03/21)=000000000000000000000000
SPRITE ROW DATA (04/21)=000000000000000000000000
SPRITE ROW DATA (05/21)=000000000000000000000000
SPRITE ROW DATA (06/21)=000000000000010000000000
SPRITE ROW DATA (07/21)=000000000000001000000000
SPRITE ROW DATA (08/21)=000000000000000100000000
SPRITE ROW DATA (09/21)=000000000000000000000000
SPRITE ROW DATA (10/21)=000000000000000000000000
SPRITE ROW DATA (11/21)=000000000000000000000000
SPRITE ROW DATA (12/21)=000000000000000000000000
SPRITE ROW DATA (13/21)=000000000000000000000000
SPRITE ROW DATA (14/21)=000000000000000000000000
SPRITE ROW DATA (15/21)=000000000000000000000000
SPRITE ROW DATA (16/21)=000000000000000000000000
SPRITE ROW DATA (17/21)=000000000000000000000000
SPRITE ROW DATA (18/21)=000000000000000000000000
SPRITE ROW DATA (19/21)=000000000000000000000000
SPRITE ROW DATA (20/21)=000000000000000000000000
SPRITE ROW DATA (21/21)=000000000000000000000000
END SPRITE (2/3)

BEGIN SPRITE (3/3)
NAME=Sprite 2 (multicolor)
MULTICOLOR=YES
PREVIEW OFFSET=30,30
EXPAND=NO
PREVIEW ZOOM=NO
COLOR PALETTE=Black-White-Red-Cyan
SPRITE ROW DATA (01/21)=000000000000
SPRITE ROW DATA (02/21)=000000000000
SPRITE ROW DATA (03/21)=000000000000
SPRITE ROW DATA (04/21)=000000000000
SPRITE ROW DATA (05/21)=000010000000
SPRITE ROW DATA (06/21)=000010000000
SPRITE ROW DATA (07/21)=000010000000
SPRITE ROW DATA (08/21)=000010000000
SPRITE ROW DATA (09/21)=000010000000
SPRITE ROW DATA (10/21)=000000000000
SPRITE ROW DATA (11/21)=000000000000
SPRITE ROW DATA (12/21)=000000000000
SPRITE ROW DATA (13/21)=000000000000
SPRITE ROW DATA (14/21)=000000000000
SPRITE ROW DATA (15/21)=000000000000
SPRITE ROW DATA (16/21)=000000000000
SPRITE ROW DATA (17/21)=000000000000
SPRITE ROW DATA (18/21)=000000000000
SPRITE ROW DATA (19/21)=000000000000
SPRITE ROW DATA (20/21)=000000000000
SPRITE ROW DATA (21/21)=000000000000
END SPRITE (3/3)

END SPRITES
END FILE";

            var spriteList = new EditStateSprite.SpriteList();
            spriteList.Deserialize(data);

            Assert.AreEqual(3, spriteList.Count);

            var s = spriteList[0];

            Assert.AreEqual(true, s.MultiColor);

            s = spriteList[1];

            Assert.AreEqual(false, s.MultiColor);

            s = spriteList[2];

            Assert.AreEqual(true, s.MultiColor);
        }
    }
}