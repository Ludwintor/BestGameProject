using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGame
{
    public class RNG
    {
        private const int MBIG = int.MaxValue;
        private const int MSEED = 0x9A4EC86;
        private const double SPREAD = 1.0D / MBIG;

        public int Seed => _seed;

        private int _seed;
        private int[] _seedArray;
        private int _iNext;
        private int _iNextp;

        #region CTORs
        public RNG() : this(Environment.TickCount) { }

        public RNG(string seed) : this(seed.GetHashCode()) { } // HDNE OEWM 36^8

        public RNG(int seed)
        {
            _seed = seed;
            _seedArray = new int[56];
            // This algorithm comes from Numerical Recipes in C (2nd edition)
            // Also original implementation of System.Random
            int subtraction = seed == int.MinValue ? int.MaxValue : Math.Abs(seed);
            int mj = MSEED - subtraction;
            _seedArray[55] = mj;
            int mk = 1;
            for (int i = 0; i < 55; i++)
            {
                int ii = 21 * i % 55;
                _seedArray[ii] = mk;
                mk = mj - mk;
                if (mk < 0)
                    mk += MBIG;
                mj = _seedArray[ii];
            }
            for (int k = 1; k < 5; k++)
                for (int i = 1; i < 56; i++)
                {
                    _seedArray[i] -= _seedArray[1 + (i + 30) % 55];
                    if (_seedArray[i] < 0)
                        _seedArray[i] += MBIG;
                }
            _iNext = 0;
            _iNextp = 21;
        }

        public RNG(int[] state)
        {
            if (state.Length != 59)
                throw new Exception("RNG state is corrupted");
            _seed = state[0];
            _iNext = state[1];
            _iNextp = state[2];
            _seedArray = new int[56];
            for (int i = 4; i < state.Length; i++)
                _seedArray[i - 3] = state[i];
        }
        #endregion

        public int[] GetState()
        {
            int[] state = new int[59];
            state[0] = _seed;
            state[1] = _iNext;
            state[2] = _iNextp;
            for (int i = 4; i < state.Length; i++)
                state[i] = _seedArray[i - 3];
            return state;
        }

        /// <param name="min">Min inclusive integer</param>
        /// <param name="max">Max exclusive integer</param>
        /// <returns>Integer value in range [min; max)</returns>
        public int NextInt(int min, int max)
        {
            int range = max - min;
            return (int)(Sample() * range) + min;
        }

        /// <param name="max">Max exclusive integer</param>
        /// <returns>Integer value in range [0; max)</returns>
        public int NextInt(int max) => NextInt(0, max);

        /// <param name="min">Min inclusive float</param>
        /// <param name="max">Max exclusive float</param>
        /// <returns>Float value in range [min; max)</returns>
        public float NextFloat(float min, float max)
        {
            float range = max - min;
            return NextFloat() * range + min;
        }

        /// <param name="max">Max exclusive float</param>
        /// <returns>Float value in range [0; max)</returns>
        public float NextFloat(float max) => NextFloat(0f, max);

        /// <returns>Float value in range [0; 1)</returns>
        public float NextFloat()
        {
            return (float)Sample();
        }

        public Vector2 NextVector2(Vector2 min, Vector2 max)
        {
            float nextX = NextFloat(min.x, max.x);
            float nextY = NextFloat(min.y, max.y);
            return new Vector2(nextX, nextY);
        }

        public Vector3 NextVector3(Vector3 min, Vector3 max)
        {
            Vector2 nextXY = NextVector2(min, max);
            float nextZ = NextFloat(min.z, max.z);
            return new Vector3(nextXY.x, nextXY.y, nextZ);
        }

        public byte NextByte()
        {
            return (byte)NextInt(byte.MinValue, byte.MaxValue + 1);
        }

        public void NextBytes(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = NextByte();
        }

        public Vector2 OnUnitCircle()
        {
            float x = NextFloat();
            float y = NextFloat();
            return new Vector2(x, y).normalized;
        }

        public Vector2 InsideUnitCircle()
        {
            float lengthFactor = NextFloat();
            return OnUnitCircle() * lengthFactor;
        }

        public T NextElement<T>(IList<T> list)
        {
            int nextInt = NextInt(list.Count);
            return list[nextInt];
        }

        /// <summary>
        /// Get pseudo-random value and reseed array
        /// </summary>
        /// <returns>Double value in range [0; 1)</returns>
        private double Sample()
        {
            if (++_iNext >= 56)
                _iNext = 1;
            if (++_iNextp >= 56)
                _iNextp = 1;
            int sample = _seedArray[_iNext] - _seedArray[_iNextp];
            if (sample == MBIG)
                sample--;
            if (sample < 0)
                sample += MBIG;
            _seedArray[_iNext] = sample;
            return sample * SPREAD;
        }
    }
}
