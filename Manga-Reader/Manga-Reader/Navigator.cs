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

        public Navigator(Container root)
        {
            this.root = root;
            FindCurrentContainer(root);
        }

        private void FindCurrentContainer(Container start)
        {
            void ResetCurrentContainerRec(Container r)
            {
                if (r.PageWrapper.Pages.Count() > 0)
                {
                    currentContainer = r;
                    return;
                }
                foreach (Container container in r.Containers)
                {
                    ResetCurrentContainerRec(container);
                    if (currentContainer != null)
                        return;
                }
            }
            currentContainer = null;
            ResetCurrentContainerRec(start);
        }

        private void FindNextCurrentContainer()
        {
            Container curr = currentContainer;
            while(true)
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
        private void FindPreviousCurrentContainer()
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
                        return;
                }
                curr = parent;
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
