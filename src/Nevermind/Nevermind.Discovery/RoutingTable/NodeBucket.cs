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

using System;
using System.Collections.Generic;
using System.Linq;

namespace Nevermind.Discovery.RoutingTable
{
    public class NodeBucket : INodeBucket
    {
        private readonly object _nodeBucketLock = new object();

        public NodeBucket(int distance, int bucketSize)
        {
            Items = new SortedSet<NodeBucketItem>(new LastContactTimeComparer());
            Distance = distance;
            BucketSize = bucketSize;
        }

        /// <summary>
        /// Distance from Master Node
        /// </summary>
        public int Distance { get; }
        public int BucketSize { get; }
        public SortedSet<NodeBucketItem> Items { get; }

        public NodeAddResult AddNode(Node node)
        {
            lock (_nodeBucketLock)
            {
                if (Items.Count < BucketSize)
                {
                    var item = new NodeBucketItem(node);
                    if (!Items.Contains(item))
                    {
                        Items.Add(item);
                    }
                    return NodeAddResult.Added();
                }

                var evictionCandidate = GetEvictionCandidate();
                return NodeAddResult.Full(evictionCandidate);
            }  
        }

        public void RemoveNode(Node node)
        {
            lock (_nodeBucketLock)
            {
                var item = new NodeBucketItem(node);
                if (Items.Contains(item))
                {
                    Items.Remove(item);
                }
            }
        }

        public void ReplaceNode(Node nodeToRemove, Node nodeToAdd)
        {
            lock (_nodeBucketLock)
            {
                var item = new NodeBucketItem(nodeToRemove);
                if (Items.Contains(item))
                {
                    Items.Remove(item);
                }
                item = new NodeBucketItem(nodeToAdd);
                if (!Items.Contains(item))
                {
                    Items.Add(item);
                }
            }
        }

        private NodeBucketItem GetEvictionCandidate()
        {
            return Items.Last();
        }

        private class LastContactTimeComparer : IComparer<NodeBucketItem>
        {
            public int Compare(NodeBucketItem x, NodeBucketItem y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }

                if (x == null)
                {
                    return -1;
                }

                if (y == null)
                {
                    return 1;
                }

                if (ReferenceEquals(x, y))
                {
                    return 0;
                }

                //checking if both objects are the same
                if (x.Equals(y))
                {
                    return 0;
                }

                var timeComparison = x.LastContactTime.CompareTo(y.LastContactTime);
                if (timeComparison == 0)
                {
                    //last contact time is the same, but items are not the same, selecting higher id as higher item
                    return x.Node.GetHashCode() > y.Node.GetHashCode() ? 1 : -1;
                }

                return timeComparison;
            }
        }
    }
}