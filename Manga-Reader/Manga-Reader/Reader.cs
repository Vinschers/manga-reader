﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Reader
{
    class Reader
    {
        protected Navigator navigator;
        protected PathWrapper pathWrapper;
        protected int pageNumber = -1;

        public Navigator Navigator { get => navigator; }
        public PathWrapper PathWrapper { get => pathWrapper; }
        public string Name { get => navigator.Root.Name; }
        public Page Page { get => navigator.Page; }
        public string PageBreaker { get => pathWrapper.PageBreaker; }

        public Reader(Navigator nav, PathWrapper pw)
        {
            navigator = nav;
            pathWrapper = pw;
            pageNumber = 0;
        }

        public void CopyToClipboard()
        {
            Clipboard.SetImage(Page.Image);
        }

        public void ChangePage(int deltaPages)
        {
            string pageBreakerValueBefore = navigator.GetContainerKey(PageBreaker).Path;
            navigator.ChangePage(deltaPages);
            Container pageBreakerAfter = navigator.GetContainerKey(PageBreaker);
            string pageBreakerValueAfter = pageBreakerAfter.Path;

            if (pageBreakerValueBefore == pageBreakerValueAfter)
                pageNumber += deltaPages;
            else
                pageNumber = navigator.GetPageNumber(PageBreaker);
        }
        public void ChangeContainer(Container container)
        {
            string pageBreakerValueBefore = navigator.GetContainerKey(PageBreaker).Path;
            navigator.ChangeContainer(container);
            Container pageBreakerAfter = navigator.GetContainerKey(PageBreaker);
            string pageBreakerValueAfter = pageBreakerAfter.Path;

            if (pageBreakerValueBefore == pageBreakerValueAfter)
                pageNumber = navigator.GetPageNumber(PageBreaker);
            else
                pageNumber = 1;
        }
        public void DeleteCurrent()
        {
            int delta = navigator.DeletePage();
            pageNumber += delta;
        }
        public void RenameKey(string key)
        {
            navigator.RenameKey(key);
        }

        public string GetConfigs()
        {
            string ret = "";
            ret += pathWrapper.Organization + "\n";
            ret += pathWrapper.Template + "\n";
            ret += pathWrapper.PageBreaker + "\n";
            return ret;
        }
        public void LoadConfigs(string path)
        {
            StreamReader sr = new StreamReader(path);

            pathWrapper.SetPathOrganization(sr.ReadLine(), Page.Parent.Path);
            pathWrapper.SetRenameTemplate(sr.ReadLine());
            pathWrapper.SetPageBreaker(sr.ReadLine());

            sr.Close();
        }
    }
}
