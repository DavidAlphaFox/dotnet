// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Description: 
//    RMEnrollmentPage2 is page 2 of the RM enrollment wizard.

using System.Windows.TrustUI;


namespace MS.Internal.Documents
{
    /// <summary>
    /// RMEnrollmentPage2 
    /// </summary>
    internal sealed partial class RMEnrollmentPage2 : DialogBaseForm
    {
        #region Constructors
        //------------------------------------------------------
        //
        //  Constructors
        //
        //------------------------------------------------------

        /// <summary>
        /// The constructor
        /// </summary>
        internal RMEnrollmentPage2()
        {
            // disabling until images are avaliable
            _pictureBox1.Visible = false;
            _pictureBox2.Visible = false;
            _pictureBox4.Visible = false;
        }

        #endregion Constructors

        #region Public properties
        //------------------------------------------------------
        //
        //  Public properties
        //
        //------------------------------------------------------

        /// <summary>
        /// Windows Account has been selected.
        /// </summary>
        public EnrollmentAccountType AccountTypeSelected
        {
            get
            {
                EnrollmentAccountType accountType = EnrollmentAccountType.None;

                if (_networkRadioButton.Checked)
                {
                    accountType = EnrollmentAccountType.Network;
                }

                if (_passportRadioButton.Checked)
                {
                    accountType = EnrollmentAccountType.NET;
                }

                return accountType;
            }
        }

        #endregion Public properties

        #region Protected Methods
        //------------------------------------------------------
        //
        //  Protected Methods
        //
        //------------------------------------------------------

        /// <summary>
        /// ApplyResources override.  Called to apply dialog resources.
        /// </summary>
        protected override void ApplyResources()
        {
            base.ApplyResources();

            this._nextButton.Text = SR.RMEnrollmentNext;
            this._cancelButton.Text = SR.RMEnrollmentCancel;
            this._label1.Text = SR.RMEnrollmentPage2a;
            this._label2.Text = SR.RMEnrollmentPage2b;
            this._networkRadioButton.Text = SR.RMEnrollmentPage2c;
            this._passportRadioButton.Text = SR.RMEnrollmentPage2e;
            this.Text = SR.RMEnrollmentTitle;
        }

        #endregion Protected Methods

    }
}