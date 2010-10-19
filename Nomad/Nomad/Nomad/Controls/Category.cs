namespace Nomad.Controls
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Runtime.CompilerServices;

    [ToolboxItem(false), DesignTimeVisible(false), DefaultProperty("Text")]
    public class Category : NamedComponent, IContainer, IDisposable
    {
        private CategoryComponentCollection ComponentCollection;
        private string FText;

        public event ComponentEventHandler ComponentAdded;

        public event ComponentEventHandler ComponentRemoved;

        [Category("Property Changed")]
        public event EventHandler TextChanged;

        public Category()
        {
            this.FText = string.Empty;
        }

        public Category(IContainer container)
        {
            this.FText = string.Empty;
            container.Add(this);
        }

        public void Add(IComponent component)
        {
            if (this.ComponentCollection == null)
            {
                this.ComponentCollection = new CategoryComponentCollection();
            }
            if (this.ComponentCollection.Add(component))
            {
                component.Disposed += new EventHandler(this.ComponentDisposed);
                this.OnComponentAdded(new ComponentEventArgs(component));
            }
        }

        public void Add(IComponent component, string name)
        {
            throw new NotImplementedException();
        }

        private void ComponentDisposed(object sender, EventArgs e)
        {
            this.Remove((IComponent) sender);
        }

        protected virtual void OnComponentAdded(ComponentEventArgs e)
        {
            if (this.ComponentAdded != null)
            {
                this.ComponentAdded(this, e);
            }
        }

        protected virtual void OnComponentRemoved(ComponentEventArgs e)
        {
            if (this.ComponentRemoved != null)
            {
                this.ComponentRemoved(this, e);
            }
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
            if (this.TextChanged != null)
            {
                this.TextChanged(this, e);
            }
        }

        public void Remove(IComponent component)
        {
            if ((this.ComponentCollection != null) && this.ComponentCollection.Remove(component))
            {
                component.Disposed -= new EventHandler(this.ComponentDisposed);
                this.OnComponentRemoved(new ComponentEventArgs(component));
            }
        }

        public override string ToString()
        {
            return this.FText;
        }

        public System.ComponentModel.ComponentCollection Components
        {
            get
            {
                if (this.ComponentCollection == null)
                {
                    this.ComponentCollection = new CategoryComponentCollection();
                }
                return this.ComponentCollection;
            }
        }

        [Localizable(true), Category("Appearance")]
        public string Text
        {
            get
            {
                return this.FText;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                if (this.FText != value)
                {
                    this.FText = value;
                    this.OnTextChanged(EventArgs.Empty);
                }
            }
        }

        private class CategoryComponentCollection : ComponentCollection
        {
            public CategoryComponentCollection() : base(new IComponent[0])
            {
            }

            public bool Add(IComponent component)
            {
                if (base.InnerList.IndexOf(component) < 0)
                {
                    base.InnerList.Add(component);
                    return true;
                }
                return false;
            }

            public bool Remove(IComponent component)
            {
                for (int i = 0; i < base.InnerList.Count; i++)
                {
                    if (base.InnerList[i] == component)
                    {
                        base.InnerList.RemoveAt(i);
                        return true;
                    }
                }
                return false;
            }
        }
    }
}

