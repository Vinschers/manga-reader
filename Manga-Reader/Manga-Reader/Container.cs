﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace Manga_Reader
{
    public class Key
    {
        public string StringValue { get; set; }
        public int NumericValue { get; set; }
    }
    public abstract class Container
    {
        protected List<Container> containers;
        protected PageWrapper pageWrapper;
        protected Container parent;
        protected string name;
        protected string path;
        protected int depth;
        protected Key key;

        public Key Key { get => key; set => key = value; }
        public string Path { get => path; }
        public string Name { get => name; }
        public int Depth { get => depth; }
        public PageWrapper PageWrapper { get => pageWrapper; }
        public List<Container> Containers { get => containers; }
        public Container Parent { get => parent; }

        public Container(Container parent)
        {
            this.parent = parent;
            containers = new List<Container>();
            depth = -1;
        }
        public Container(Container parent, string path) : this(parent)
        {
            this.path = path;
            name = path.Substring(path.LastIndexOf("\\") + 1);

            Reset();
            GetDepth();
        }
        public Container(string path)
        {
            containers = new List<Container>();
            this.path = path;
            name = path.Substring(path.LastIndexOf("\\") + 1);
            depth = -1;

            Reset();
            GetDepth();
        }
        public Container(Container parent, string path, Key key) : this(parent, path)
        {
            this.key = key;
        }

        public void Delete()
        {
            foreach (Container container in containers)
                container.Delete();

            containers.Clear();
            pageWrapper.Delete();
        }

        protected abstract void Reset();

        public int PagesCount(int n)
        {
            foreach (Container container in containers)
                n += container.PagesCount(0);
            n += pageWrapper.Pages.Count();
            return n;
        }

        public int CountPagesUntil(Container end)
        {
            if (Equals(end))
                return 0;

            int n = pageWrapper.Pages.Count();
            foreach (Container container in containers)
            {
                if (container.Equals(end))
                    break;
                n += container.PagesCount(0);
            }

            return n;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Container))
                return false;
            Container c = (Container)obj;
            if (c.Path != this.Path)
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = -1904681517;
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Container>>.Default.GetHashCode(containers);
            hashCode = hashCode * -1521134295 + EqualityComparer<PageWrapper>.Default.GetHashCode(pageWrapper);
            hashCode = hashCode * -1521134295 + EqualityComparer<Container>.Default.GetHashCode(parent);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(path);
            return hashCode;
        }

        protected void GetDepth()
        {
            depth = 0;

            void GetDepthRec(Container root, int current)
            {
                if (current > depth)
                    depth = current;
                current++;
                foreach(Container cont in root.containers)
                    GetDepthRec(cont, current);
            }

            GetDepthRec(this, 0);
        }
    }
}
