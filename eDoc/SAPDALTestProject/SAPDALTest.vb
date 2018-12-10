Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports SAPDAL



'''<summary>
'''This is a test class for SAPDALTest and is intended
'''to contain all SAPDALTest Unit Tests
'''</summary>
<TestClass()> _
Public Class SAPDALTest


    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(value As TestContext)
            testContextInstance = Value
        End Set
    End Property

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    '<ClassInitialize()>  _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    'Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()>  _
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    'Use TestInitialize to run code before running each test
    '<TestInitialize()>  _
    'Public Sub MyTestInitialize()
    'End Sub
    '
    'Use TestCleanup to run code after each test has run
    '<TestCleanup()>  _
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region


    '''<summary>
    '''A test for GetMultiPrice_eStore
    '''</summary>
    <TestMethod(), _
     DeploymentItem("SAPDAL.dll")> _
    Public Sub GetMultiPrice_eStoreTest()
        Dim Org As String = "US01" 'String.Empty ' TODO: Initialize to an appropriate value
        Dim SoldToId As String = "UAGI5001" ' String.Empty ' TODO: Initialize to an appropriate value
        Dim ShipToId As String = "UAGI5001" 'String.Empty ' TODO: Initialize to an appropriate value
        Dim Currency As String = "USD" ' String.Empty ' TODO: Initialize to an appropriate value
        Dim strDistChann As String = "10" ' String.Empty ' TODO: Initialize to an appropriate value
        Dim strDivision As String = "20" ' String.Empty ' TODO: Initialize to an appropriate value
        Dim ProductIn As SAPDAL.SAPDALDS.ProductInDataTable = New SAPDAL.SAPDALDS.ProductInDataTable ' TODO: Initialize to an appropriate value
        ProductIn.AddProductInRow("ADAM-4520-EE", 1, "")

        Dim ProductOut As SAPDAL.SAPDALDS.ProductOutDataTable = New SAPDAL.SAPDALDS.ProductOutDataTable ' TODO: Initialize to an appropriate value
        Dim ProductOutExpected As SAPDAL.SAPDALDS.ProductOutDataTable = New SAPDAL.SAPDALDS.ProductOutDataTable ' TODO: Initialize to an appropriate value
        Dim ErrorMessage As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim ErrorMessageExpected As String = String.Empty ' TODO: Initialize to an appropriate value
        Dim expected As Boolean = False ' TODO: Initialize to an appropriate value
        Dim actual As Boolean
        actual = SAPDAL_Accessor.GetMultiPrice_eStore(Org, SoldToId, ShipToId, Currency, strDistChann, strDivision, ProductIn, ProductOut, ErrorMessage)

        Dim _UnitPrice As Decimal = CType(ProductOut.Rows(0), SAPDALDS.ProductOutRow).UNIT_PRICE
        'Assert.AreNotEqual(0, _UnitPrice)
        Assert.AreNotEqual(0, _UnitPrice)
        'Assert.in()
        'Assert.AreEqual(ProductOutExpected, ProductOut)
        'Assert.AreEqual(ErrorMessageExpected, ErrorMessage)
        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
