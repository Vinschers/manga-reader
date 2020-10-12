using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Reader
{
    class Navigator
    {
        Container root;
        Container currentContainer;

        public Container CurrentContainer { get => currentContainer; }
        public Page Page { get => currentContainer.PageWrapper.CurrentPage; }
        public Container Root { get => root; }

        public Navigator(Container root)
        {
            this.root = root;
            FindCurrentContainer(root);
        }

        private void FindCurrentContainer(Container start)
        {
            void FindCurrentContainerRec(Container r)
            {
                if (r.PageWrapper.Pages.Count() > 0)
                {
                    currentContainer = r;
                    return;
                }
                foreach (Container container in r.Containers)
                {
                    FindCurrentContainerRec(container);
                    if (currentContainer != null)
                        return;
                }
            }
            currentContainer = null;
            FindCurrentContainerRec(start);
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
                        FindCurrentContainer(parent.Containers.ElementAt(i));
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
                    for (int i = startIndex; i > 0; i--)
                    {
                        FindCurrentContainer(parent.Containers.ElementAt(i));
                        if (currentContainer != null)
                        {
                            currentContainer.PageWrapper.SetPage(-1);
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

        public void ChangePage(int n)
        {
            if (n == 0)
                return;

            var rest = currentContainer.PageWrapper.ChangePage(n);

            if (rest > 0)
                FindNextCurrentContainer();
            else if (rest < 0)
                FindPreviousCurrentContainer();

            ChangePage(rest);
        }

        public void ChangeContainer(Container newContainer)
        {
            FindCurrentContainer(newContainer);
            if (currentContainer == null)
                throw new Exception("Container not found");
        }

        public void DeletePage()
        {
            currentContainer.PageWrapper.DeleteCurrentPage();
        }
    }
}
