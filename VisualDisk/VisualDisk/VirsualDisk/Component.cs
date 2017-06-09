﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDisk
{
    public class Component
    {
        protected string _name;
        protected Component _parent;
        protected List<Component> _childs = new List<Component>();

        public Component(string name)
        {
            _name = name;
        }

        public virtual void Dispose()
        {
            _parent = null;
            _childs.Clear();
        }

        public virtual void Add(Component child)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove()
        {
            throw new NotImplementedException();
        }

        public virtual string GetPath()
        {
            StringBuilder sb = new StringBuilder(_name);
            Component p = _parent;
            while (p != null)
            {
                sb.Insert(0, p._name + @"\");
                p = p._parent;
            }
            return sb.ToString();
        }

        public Component GetChild(string name)
        {
            foreach (Component child in _childs)
            {
                if (child.name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    return child;
            }
            return null;
        }

        public void SetParent(Component parent)
        {
            _parent = parent;
        }

        public string name
        { get { return _name; } }

        public Component parent
        { get { return _parent; } }

        public List<Component> childs
        { get { return _childs; } }
    }
}