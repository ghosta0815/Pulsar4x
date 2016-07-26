﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulsar4X.ECSLib
{
    public static class AsteroidFactory
    {
        /// <summary>
        /// creates an asteroid that will collide with the given entity on the given date. 
        /// </summary>
        /// <param name="starSys"></param>
        /// <param name="target"></param>
        /// <param name="collisionDate"></param>
        /// <returns></returns>
        public static Entity CreateAsteroid(StarSystem starSys, Entity target, DateTime collisionDate)
        {
            //todo rand these a bit.
            double radius = Distance.ToAU(500.0);
            double mass = 1.5e+12; //about 1.5 billion tonne
            Vector4 velocity = new Vector4(8, 7, 0, 0);

            var position = new PositionDB(0, 0, 0, Guid.Empty);
            var massVolume = MassVolumeDB.NewFromMassAndRadius(mass, radius);
            var planetInfo = new SystemBodyDB();
            var balisticTraj = new NewtonBalisticDB();
            var name = new NameDB("Ellie");

            planetInfo.SupportsPopulations = false;
            planetInfo.Type = BodyType.Asteroid;

            Vector4 targetPos = OrbitProcessor.GetAbsolutePosition(target.GetDataBlob<OrbitDB>(), collisionDate);
            TimeSpan timeToCollision = starSys.Game.CurrentDateTime - collisionDate;
            targetPos -= velocity * timeToCollision.Seconds;
            position.AbsolutePosition = targetPos;
            balisticTraj.CurrentSpeed = velocity;

            var planetDBs = new List<BaseDataBlob>
            {
                position,
                massVolume,
                planetInfo,
                name,
                balisticTraj
            };

            Entity newELE = new Entity(starSys.SystemManager, planetDBs);
            return newELE;
        }
    }
}