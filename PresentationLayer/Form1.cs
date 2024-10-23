using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PresentationLayer
{
    public partial class Form1 : Form
    {
        private readonly ProductRepository _productRepository;

        public Form1()
        {
            InitializeComponent();
            Settings.License = LicenseType.Community;
            _productRepository = new ProductRepository();
            GetProducts();
        }

        public void GetProducts()
        {
            dgvProducts.DataSource = _productRepository.GetProducts();
        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            var productList = new List<Products>();

            foreach (DataGridViewRow row in dgvProducts.Rows)
            {
                if (row.IsNewRow) continue;

                var products = new Products
                {
                    ProductID = Convert.ToInt32(row.Cells[0].Value),
                    ProductName = row.Cells[1].Value.ToString(),
                    QuantityPerUnit = row.Cells[2].Value.ToString(),
                    UnitPrice = Convert.ToDecimal(row.Cells[3].Value),
                    UnitsInStock = Convert.ToInt32(row.Cells[4].Value)
                };

                productList.Add(products);
            }

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(1, Unit.Centimetre);

                    page.Header().Height(35).Background(Colors.Green.Accent4)
                    .Text("Reporte de Productos").Bold().AlignCenter().FontSize(20)
                    .FontColor(Colors.White);

                    page.Content()
                    .Column(column =>
                    {
                        column.Item().PaddingLeft(1, Unit.Centimetre).PaddingTop(1, Unit.Centimetre)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            foreach (var product in productList)
                            {
                                table.Cell().Text(product.ProductID);
                                table.Cell().Text(product.ProductName);
                                table.Cell().Text(product.QuantityPerUnit);
                                table.Cell().Text(product.UnitPrice);
                                table.Cell().Text(product.UnitsInStock);
                            }
                        });
                    });
                });
            }).GeneratePdfAndShow();
            MessageBox.Show("Reporte Generado con Exito");
        }
    }
}
