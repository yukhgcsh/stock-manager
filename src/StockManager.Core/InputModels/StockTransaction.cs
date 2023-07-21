using StockManager.Core.Utils;

namespace StockManager.Core.InputModels
{
    /// <summary>
    ///     �����̎�������`���܂��B
    /// </summary>
    public class StockTransaction
    {
        /// <summary>
        ///     �����R�[�h���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        ///     ��Ж����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        ///     ����������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        ///     �������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public string? Memo { get; set; }

        /// <summary>
        ///     ������t���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     ����^�C�v���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public TransactionType Type { get; set; }

        /// <summary>
        ///     ������i���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        public double Price { get; set; }
    }
}
