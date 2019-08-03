using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FamilyCarePro
{
    internal class LineReader : IEnumerable<string>
    {
        private Func<StringReader> p;

        public LineReader(Func<StringReader> p)
        {
            this.p = p;
        }

        public IEnumerator<string> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}