namespace Nomad.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design;

    [ToolboxItemFilter("System.Windows.Forms"), ProvideProperty("Category", typeof(IComponent)), DefaultProperty("CategoryList")]
    public class CategoryManager : Component, IExtenderProvider
    {
        private Dictionary<IComponent, Category> ComponentCategoryMap;
        private Nomad.Controls.CategoryList FCategoryList;

        public CategoryManager()
        {
            this.FCategoryList = new Nomad.Controls.CategoryList();
            this.ComponentCategoryMap = new Dictionary<IComponent, Category>();
        }

        public CategoryManager(IContainer container)
        {
            this.FCategoryList = new Nomad.Controls.CategoryList();
            this.ComponentCategoryMap = new Dictionary<IComponent, Category>();
            container.Add(this);
        }

        public bool CanExtend(object extendee)
        {
            return (((extendee is IComponent) && !(extendee is CategoryManager)) && !(extendee is Category));
        }

        private void CategoryComponentAdded(object sender, ComponentEventArgs e)
        {
            e.Component.Disposed += new EventHandler(this.ComponentDisposed);
            this.ComponentCategoryMap.Add(e.Component, (Category) sender);
        }

        private void CategoryComponentRemoved(object sender, ComponentEventArgs e)
        {
            this.RemoveComponent(e.Component);
            Category category = (Category) sender;
            if (!this.ComponentCategoryMap.ContainsValue(category))
            {
                category.ComponentAdded -= new ComponentEventHandler(this.CategoryComponentAdded);
                category.ComponentRemoved -= new ComponentEventHandler(this.CategoryComponentRemoved);
                category.Disposed -= new EventHandler(this.CategoryDisposed);
            }
        }

        private void CategoryDisposed(object sender, EventArgs e)
        {
            List<IComponent> list = new List<IComponent>();
            foreach (KeyValuePair<IComponent, Category> pair in this.ComponentCategoryMap)
            {
                if (pair.Value == sender)
                {
                    list.Add(pair.Key);
                }
            }
            foreach (Component component in list)
            {
                this.RemoveComponent(component);
            }
            ((Category) sender).Disposed -= new EventHandler(this.CategoryDisposed);
        }

        private void ComponentDisposed(object sender, EventArgs e)
        {
            this.RemoveComponent((IComponent) sender);
        }

        [DefaultValue(typeof(Category), "")]
        public Category GetCategory(IComponent component)
        {
            Category category;
            if (this.ComponentCategoryMap.TryGetValue(component, out category))
            {
                return category;
            }
            return null;
        }

        private void RemoveComponent(IComponent component)
        {
            component.Disposed -= new EventHandler(this.ComponentDisposed);
            this.ComponentCategoryMap.Remove(component);
        }

        public void SetCategory(IComponent component, Category category)
        {
            Category category2;
            if (this.ComponentCategoryMap.TryGetValue(component, out category2))
            {
                if (category2 == category)
                {
                    return;
                }
                category2.Remove(component);
            }
            if (category != null)
            {
                if (!this.ComponentCategoryMap.ContainsValue(category))
                {
                    category.ComponentAdded += new ComponentEventHandler(this.CategoryComponentAdded);
                    category.ComponentRemoved += new ComponentEventHandler(this.CategoryComponentRemoved);
                    category.Disposed += new EventHandler(this.CategoryDisposed);
                }
                category.Add(component);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Nomad.Controls.CategoryList CategoryList
        {
            get
            {
                return this.FCategoryList;
            }
        }
    }
}

