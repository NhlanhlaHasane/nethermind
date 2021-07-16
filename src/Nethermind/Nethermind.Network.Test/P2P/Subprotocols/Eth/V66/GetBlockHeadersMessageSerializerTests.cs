//  Copyright (c) 2021 Demerzel Solutions Limited
//  This file is part of the Nethermind library.
// 
//  The Nethermind library is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  The Nethermind library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU Lesser General Public License for more details.
// 
//  You should have received a copy of the GNU Lesser General Public License
//  along with the Nethermind. If not, see <http://www.gnu.org/licenses/>.
// 

using Nethermind.Core.Crypto;
using Nethermind.Network.P2P.Subprotocols.Eth.V66;
using Nethermind.Network.Test.P2P.Subprotocols.Eth.V62;
using NUnit.Framework;

namespace Nethermind.Network.Test.P2P.Subprotocols.Eth.V66
{
    [TestFixture]
    public class GetBlockHeadersMessageSerializerTests
    {
        //test from https://github.com/ethereum/EIPs/blob/master/EIPS/eip-2481.md
        [Test]
        public void RoundTrip_number()
        {
            var ethMessage = new Network.P2P.Subprotocols.Eth.V62.GetBlockHeadersMessage();
            ethMessage.StartBlockHash = null;
            ethMessage.StartBlockNumber = 9999;
            ethMessage.MaxHeaders = 5;
            ethMessage.Skip = 5;
            ethMessage.Reverse = 0;

            var message = new GetBlockHeadersMessage(1111, ethMessage);

            GetBlockHeadersMessageSerializer serializer = new GetBlockHeadersMessageSerializer(new Network.P2P.Subprotocols.Eth.V62.GetBlockHeadersMessageSerializer());

            SerializerTester.TestZero(serializer, message, "ca820457c682270f050580");
        }
        
        //test from https://github.com/ethereum/EIPs/blob/master/EIPS/eip-2481.md
        [Test]
        public void RoundTrip_hash()
        {
            var ethMessage = new Network.P2P.Subprotocols.Eth.V62.GetBlockHeadersMessage();
            ethMessage.StartBlockHash = new Keccak("0x00000000000000000000000000000000000000000000000000000000deadc0de");
            ethMessage.StartBlockNumber = 0;
            ethMessage.MaxHeaders = 5;
            ethMessage.Skip = 5;
            ethMessage.Reverse = 0;

            var message = new GetBlockHeadersMessage(1111, ethMessage);

            GetBlockHeadersMessageSerializer serializer = new GetBlockHeadersMessageSerializer(new Network.P2P.Subprotocols.Eth.V62.GetBlockHeadersMessageSerializer());

            SerializerTester.TestZero(serializer, message, "e8820457e4a000000000000000000000000000000000000000000000000000000000deadc0de050580");
        }
    }
}
