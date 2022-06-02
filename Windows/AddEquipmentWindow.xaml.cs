using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EquipmentRent3ISP9_7.EF;
using Microsoft.Win32;

namespace EquipmentRent3ISP9_7.Windows
{
    
    
   
    public partial class AddEquipmentWindow : Window
    {
        private bool isEdit;
        Product editProduct = new Product();
        private string pathPhoto = null;

        public AddEquipmentWindow()
        {
            InitializeComponent();

            isEdit = false;
        }

        public AddEquipmentWindow(Product product)
        {
            InitializeComponent();

            // Заполнение полей свойствами  Employee 
            txtProductName.Text = product.ProductName;
            txtManufacturerName.Text = product.Manufacturer.ManufacturerName;
            txtPrice.Text = product.Price.ToString();
            dpWarranty.SelectedDate = product.Warranty;

            if (product.Photo != null)
            {
                using (MemoryStream stream = new MemoryStream(product.Photo))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();

                    productPhoto.Source = bitmapImage;
                }
            }

            tbTitle.Text = "Изменение данных оборудования";
            btnAddNew.Content = "Сохранить";

            isEdit = true;
            // Сохраняем employee для доступа вне конструктора
            editProduct = product;
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            #region Validation
            // Null or White Space
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Поле наименование пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtManufacturerName.Text))
            {
                MessageBox.Show("Поле поставщик пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Поле стоимость пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(dpWarranty.SelectedDate.ToString()))
            {
                MessageBox.Show("Поле гарантия пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            decimal savePrice = 0;
            try
            {
                savePrice = Convert.ToDecimal(txtPrice.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Стоимость введена некорректно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (savePrice <= 0)
            {
                MessageBox.Show("Стоимость введена некорректно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            #endregion

            if (isEdit)
            {
                // Обработка случайного нажатия
                var resClick = MessageBox.Show("Изменить параметры оборудования?", "Подтверждение редактирования", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resClick == MessageBoxResult.No)
                {
                    return;
                }
                try
                {
                    editProduct.ProductName = txtProductName.Text;
                    editProduct.Manufacturer.ManufacturerName = txtManufacturerName.Text;
                    editProduct.Price = savePrice;
                    editProduct.Warranty = Convert.ToDateTime(dpWarranty.SelectedDate);

                    if (pathPhoto != null)
                    {
                        editProduct.Photo = File.ReadAllBytes(pathPhoto);
                    }

                    HelperClass.HelperCl.Context.SaveChanges();
                    MessageBox.Show("Параметры оборудования изменены!", "Редактирование");
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка!");
                }
            }
            else
            {
                // Обработка случайного нажатия
                MessageBoxResult resClick = MessageBox.Show("Добавить пользователя?", "Подтверждение добавления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resClick == MessageBoxResult.No)
                {
                    return;
                }

                try
                {
                   

                    HelperClass.HelperCl.Context.SaveChanges();

                    MessageBox.Show("Пользователь добавлен!", "Добавление");
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка!");
                }
            }
            
        }

        private void btnChoosePhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                
                pathPhoto = openFile.FileName;
            }
        }
    }
}
