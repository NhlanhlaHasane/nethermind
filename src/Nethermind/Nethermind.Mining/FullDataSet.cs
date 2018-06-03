﻿/*
 * Copyright (c) 2018 Demerzel Solutions Limited
 * This file is part of the Nethermind library.
 *
 * The Nethermind library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The Nethermind library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with the Nethermind. If not, see <http://www.gnu.org/licenses/>.
 */

namespace Nethermind.Mining
{
    public class FullDataSet : IEthashDataSet
    {
        public uint[][] Data { get; set; }
        
        public uint Size => (uint)(Data.Length * Ethash.HashBytes);

        public FullDataSet(ulong setSize, IEthashDataSet cache)
        {
            //Console.WriteLine($"building data set of length {setSize}"); // TODO: temp, remove
            Data = new uint[(uint)(setSize / Ethash.HashBytes)][];
            for (uint i = 0; i < Data.Length; i++)
            {
                if (i % 100000 == 0)
                {
                    //Console.WriteLine($"building data set of length {setSize}, built {i}"); // TODO: temp, remove
                }

                Data[i] = cache.CalcDataSetItem(i);
            }
        }

        public uint[] CalcDataSetItem(uint i)
        {
            return Data[i];
        }
    }
}