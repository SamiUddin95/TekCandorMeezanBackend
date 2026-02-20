using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TekCandor.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ChequedepositInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chequedepositInfos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CycleCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    serialNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderBankCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderBranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverBankCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverBranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChequeNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldChequeNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SequenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IQATag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentSkew = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Piggyback = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageToolight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HorizontalStreaks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BelowMinimumCompressedImageSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AboveMaximumCompressedImageSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UndersizeImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoldedorTornDocumentCorners = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FoldedOrTornDocumentEdges = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FramingError = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OversizeImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageTooDark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FrontRearDimensionMismatch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarbonStrip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutOfFocus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QRCodeMatch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MICRPresent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StandardCheque = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstrumentDuplicate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AverageChequeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalChequesCount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalChequesReturnCount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaptureAtNIFT_Branch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeferredCheque = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchRemarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FraudChequeHistory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imgF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imgB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imgU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    poStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Export = table.Column<bool>(type: "bit", nullable: true),
                    serviceRun = table.Column<bool>(type: "bit", nullable: false),
                    Returnreasone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostRestriction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Error = table.Column<bool>(type: "bit", nullable: false),
                    stan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HubCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrProcORRecTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostingTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrRecTimeCCU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrRecTimeBranch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Callback = table.Column<bool>(type: "bit", nullable: true),
                    Callbacksend = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CallbackEmailSend = table.Column<bool>(type: "bit", nullable: false),
                    isEditing = table.Column<bool>(type: "bit", nullable: false),
                    chqRetSMSSend = table.Column<bool>(type: "bit", nullable: false),
                    chqRecSMSSend = table.Column<bool>(type: "bit", nullable: false),
                    ErrorInFields = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstrumentNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountBalance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IBAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoreFTId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchStaffId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproverId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorizerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Importtime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    manual_imp = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chequedepositInfos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chequedepositInfos");
        }
    }
}
