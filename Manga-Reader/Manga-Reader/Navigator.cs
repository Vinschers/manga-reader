using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Reader
{
    public class Navigator
    {
        protected Container root;
        protected Container currentContainer;

        public Container CurrentContainer { get => currentContainer; }
        public Page Page { get => currentContainer.PageWrapper.CurrentPage; }
        public Container Root { get => root; }

        public Navigator(Container root)
        {
            this.root = root;
            currentContainer = FindCurrentContainer(root);
        }

        public static Container FindCurrentContainer(Container start)
        {
            Container ret = null;
            void FindCurrentContainerRec(Container r)
            {
                if (r.PageWrapper.Pages.Count() > 0)
                {
                    ret = r;
                    return;
                }
                foreach (Container container in r.Containers)
                {
                    FindCurrentContainerRec(container);
                    if (ret != null)
                        return;
                }
            }
            
            FindCurrentContainerRec(start);
            return ret;
        }

        private void FindNextCurrentContainer()
        {
            try
            {
                Container curr = currentContainer;
                while (true)
                {
                    Container parent = curr.Parent;
                    int startIndex = parent.Containers.IndexOf(curr) + 1;
                    for (int i = startIndex; i < parent.Containers.Count(); i++)
                    {
                        currentContainer = FindCurrentContainer(parent.Containers.ElementAt(i));
                        if (currentContainer != null)
                            return;
                    }
                    curr = parent;
                }
            }
            catch
            {
                throw new Exception("End reached!");
            }
        }
        private void FindPreviousCurrentContainer()
        {
            try
            {
                Container curr = currentContainer;
                while (true)
                {
                    Container parent = curr.Parent;
                    int startIndex = parent.Containers.IndexOf(curr) - 1;
                    for (int i = startIndex; i >= 0; i--)
                    {
                        currentContainer = FindCurrentContainer(parent.Containers.ElementAt(i));
                        if (currentContainer != null)
                        {
                            SetPage(parent.Containers.ElementAt(i), - 1);
                            return;
                        }
                    }
                    curr = parent;
                }
            }
            catch
            {
                throw new Exception("Start reached!");
            }
        }
        public Container GetContainerKey(string key)
        {
            Container cont = currentContainer;

            while (cont.Key != key)
                cont = cont.Parent;

            return cont;
        }
        public int GetPageNumber(string key)
        {
            Container cKey = GetContainerKey(key);
            Container current = currentContainer;
            int page = current.PageWrapper.Pages.IndexOf(Page) + 1; //start in 1 not 0

            do
            {
                var parent = current.Parent;

                for (int i = 0; i < parent.Containers.IndexOf(current); i++)
                    page += parent.Containers.ElementAt(i).PagesCount(0);

                page += parent.PageWrapper.Pages.Count();

                current = parent;
            } while (current.Key != cKey.Key);

            return page;
        }
        public void SetPage(Container root, int n)
        {
            if (n < 0)
                n = root.PagesCount(0) + n;
            int counter = root.PageWrapper.Pages.Count();
            if (counter > n)
            {
                currentContainer = root;
                currentContainer.PageWrapper.SetPage(n);
                return;
            }

            foreach(Container c in root.Containers)
            {
                counter += c.PagesCount(0);
                if (counter > n)
                {
                    currentContainer = c;
                    currentContainer.PageWrapper.SetPage(counter - n);
                    return;
                }
            }
        }

        public void ChangePage(int n)
        {
            if (n == 0)
                return;

            var rest = currentContainer.PageWrapper.ChangePage(n);

            if (rest > 0)
            {
                FindNextCurrentContainer();
                int count = currentContainer.PagesCount(0);
                if (count > rest)
                    rest = 0;
                else
                    rest -= count;
            }
            else if (rest < 0)
            {
                FindPreviousCurrentContainer();
                int count = currentContainer.PagesCount(0);
                if (count > Math.Abs(rest))
                    rest = 0;
                else
                    rest += count;
            }

            ChangePage(rest);
        }

        public void ChangeContainer(Container newContainer)
        {
            currentContainer = FindCurrentContainer(newContainer);
            if (currentContainer == null)
                throw new Exception("Container not found");
        }

        public int DeletePage()
        {
            return currentContainer.PageWrapper.DeleteCurrentPage();
        }
    }
}
