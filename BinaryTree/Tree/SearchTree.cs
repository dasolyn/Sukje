using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTree {
    /// <summary>
    /// 기본적인 기능을 가진 탐색 트리를 나타냅니다.
    /// </summary>
    abstract class SearchTree<T> : BinaryTree<T>, IEnumerable<T> where T : IComparable<T> {
        /// <summary>
        /// 탐색 트리에 데이터를 삽입합니다.
        /// </summary>
        public abstract void Insert(T Data);
        /// <summary>
        /// 탐색 트리에서 주어진 데이터와 정렬 순서가 동일한 노드의 데이터를 찾아 반환합니다.
        /// </summary>
        /// <exception cref="ArgumentException">해당 값을 갖는 노드를 찾는데 실패하였습니다.</exception>
        public abstract T Search(T Data);
        /// <summary>
        /// 탐색 트리에서 주어진 데이터와 정렬 순서가 동일한 노드를 삭제합니다.
        /// </summary>
        /// <exception cref="ArgumentException">해당 값을 갖는 노드를 찾는데 실패하였습니다.</exception>
        public abstract void Delete(T Data);
        /// <summary>
        /// 현재 탐색 트리의 데이터를 정렬 순서로 순회하여 열거합니다. 탐색 트리의 기본 열거자입니다.
        /// </summary>
        public IEnumerable<T> AsSortedEnumerable() {
            foreach (var i in InternalInorderedEnum(Root)) yield return i.Data;
        }
        public IEnumerator<T> GetEnumerator() {
            foreach (var i in InternalInorderedEnum(Root)) yield return i.Data;
        }
        IEnumerator IEnumerable.GetEnumerator() {
            foreach (var i in InternalInorderedEnum(Root)) yield return i.Data;
        }
    }
}
