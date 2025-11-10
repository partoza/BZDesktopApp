window.SetupSupplierJS = () => {
    // Phone number formatting
    const phoneInput = document.querySelector('input[name="phone_number"]');
    if (phoneInput) {
        phoneInput.addEventListener('input', function (e) {
            // Remove any non-digit characters
            let value = e.target.value.replace(/\D/g, '');

            // Format as (XXX) XXX-XXXX for US numbers, or keep as is for international
            if (value.length <= 10) {
                if (value.length > 3 && value.length <= 6) {
                    value = `(${value.slice(0, 3)}) ${value.slice(3)}`;
                } else if (value.length > 6) {
                    value = `(${value.slice(0, 3)}) ${value.slice(3, 6)}-${value.slice(6, 10)}`;
                }
            }

            e.target.value = value;
        });
    }

    // Email validation enhancement
    const emailInput = document.querySelector('input[name="email"]');
    if (emailInput) {
        emailInput.addEventListener('blur', function (e) {
            const email = e.target.value;
            if (email && !isValidEmail(email)) {
                e.target.setCustomValidity('Please enter a valid email address');
                e.target.reportValidity();
            } else {
                e.target.setCustomValidity('');
            }
        });
    }

    // Company name auto-format (capitalize each word)
    const companyNameInput = document.querySelector('input[name="company_name"]');
    if (companyNameInput) {
        companyNameInput.addEventListener('blur', function (e) {
            if (e.target.value) {
                e.target.value = e.target.value.replace(/\b\w/g, l => l.toUpperCase());
            }
        });
    }

    // Helper function for email validation
    function isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    console.log('Supplier JS initialized successfully');
};