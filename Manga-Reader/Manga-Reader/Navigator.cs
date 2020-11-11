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
                if (r.PageWrapper.GetPagesCount() > 0)
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
            ret.PageWrapper.SetPage(1);
            return ret;
        }

        public static Container FindLastCurrentContainer(Container start)
        {
            Container ret = null;
            void FindCurrentContainerRec(Container r)
            {
                var containers = new List<Container>(r.Containers);
                containers.Reverse();
                for(int i = 0; i < containers.Count(); i++)
                {
                    FindCurrentContainerRec(containers[i]);
                    if (ret != null)
                        return;
                }

                if (r.PageWrapper.Pages.Count() > 0)
                {
                    ret = r;
                    return;
                }
            }

            FindCurrentContainerRec(start);
            ret.PageWrapper.SetPage(-1);
            return ret;
        }

        public void Delete()
        {
            root.Delete();
        }

        private void FindNextCurrentContainer()
        {
            try
            {
                Container curr = currentContainer;
                if (curr.Containers.Count() > 0)
                {
                    do
                    {
                        curr = curr.Containers.First();
                    } while (curr.PageWrapper.Pages.Count() == 0);

                    currentContainer = curr;
                    return;
                }
                
                while (true)
                {
                    while (curr.Parent.Containers.IndexOf(curr) == curr.Parent.Containers.Count() - 1 && curr != null)
                        curr = curr.Parent;
                    if (curr == null)
                        throw new Exception();

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
                    while (curr != null && curr.Parent != null && curr.Parent.Containers.IndexOf(curr) == 0 && curr.Parent.PageWrapper.Pages.Count() == 0)
                        curr = curr.Parent;
                    if (curr == null)
                        throw new Exception();

                    if (curr.Parent == null)
                    {
                        currentContainer = curr;
                        return;
                    }
                    if (curr.Parent.Containers.IndexOf(curr) == 0 && curr.Parent.PageWrapper.Pages.Count() > 0)
                    {
                        currentContainer = curr.Parent;
                        return;
                    }

                    Container parent = curr.Parent;
                    int startIndex = parent.Containers.IndexOf(curr) - 1;

                    for (int i = startIndex; i >= 0; i--)
                    {
                        currentContainer = FindLastCurrentContainer(parent.Containers.ElementAt(i));
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
        public Container GetContainerKey(Container start, Key key)
        {
            Container cont = start;

            try
            {
                while (cont.Key.NumericValue != key.NumericValue)
                    cont = cont.Parent;
            }
            catch (NullReferenceException ex)
            {
                cont = null;

                void TestKey(Container current)
                {
                    if (current.Key == key)
                        cont = current;
                    foreach (Container c in current.Containers)
                    {
                        if (cont != null)
                            return;

                        TestKey(c);
                    }
                }
                TestKey(start);
            }

            return cont;
        }
        public int GetPageNumber(Key key)
        {
            Container cKey = GetContainerKey(currentContainer, key);
            Container current = currentContainer;

            if (cKey.Key.NumericValue <= current.Key.NumericValue)
                return 0;

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
                n = root.PagesCount(0) + n + 1;
            else
                n++;
            int counter = root.PageWrapper.Pages.Count();
            if (counter >= n)
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
            if (n > 0)
                AdvancePages(n);
            else if (n < 0)
                RetreatPages(Math.Abs(n));
        }

        private void AdvancePages(int n)
        {
            while (n > 0)
            {
                if (currentContainer.PageWrapper.Pages.Count() > 0)
                {
                    List<Page> curPages = currentContainer.PageWrapper.Pages;
                    if (curPages.IndexOf(Page) + n < curPages.Count())
                    {
                        currentContainer.PageWrapper.ChangePage(n);
                        return;
                    }

                    n -= curPages.Count();
                }
                
                if (currentContainer.Containers.Count() > 0)
                {
                    currentContainer = currentContainer.Containers.First();
                    continue;
                }

                FindNextCurrentContainer();
            }
        }

        private void RetreatPages(int n)
        {
            while (n > 0)
            {
                if (currentContainer.PageWrapper.Pages.Count() > 0)
                {
                    List<Page> curPages = currentContainer.PageWrapper.Pages;
                    if (curPages.IndexOf(Page) - n >= 0)
                    {
                        currentContainer.PageWrapper.ChangePage(-n);
                        return;
                    }

                    n -= curPages.IndexOf(Page) + 1;
                }

                FindPreviousCurrentContainer();
            }
        }

        public void ChangeContainer(Container newContainer)
        {
            currentContainer = FindCurrentContainer(newContainer);
            if (currentContainer == null)
                throw new Exception("Container not found");
        }

        public int DeletePage()
        {
            return currentContainer.PageWrapper.DeletCurrentPage();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Navigator))
                return false;
            Navigator nav = obj as Navigator;
            if (!root.Equals(nav.root))
                return false;
            return true;
        }
        public Container GetDeepestContainer()
        {
            int depth = root.Depth;
            Container cont = root;

            while(depth != 0)
            {
                cont = cont.Containers.OrderBy(c => c.Depth).Last();
                depth = cont.Depth;
            }

            return cont;
        }
    }
}
