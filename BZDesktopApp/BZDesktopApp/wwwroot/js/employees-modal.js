// Password toggle functionality
function initPasswordToggle() {
    const togglePasswordBtn = document.getElementById('togglePassword');
    if (togglePasswordBtn) {
 togglePasswordBtn.addEventListener('click', function() {
            const passwordInput = document.getElementById('password');
      const eyeIcon = document.getElementById('eyeIcon');
            
     if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
        eyeIcon.innerHTML = `<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21" />`;
            } else {
           passwordInput.type = 'password';
    eyeIcon.innerHTML = `<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
           <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.477 0 8.268 2.943 9.542 7-1.274 4.057-5.065 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />`;
            }
        });
    }
}

// Confirm password toggle functionality
function initConfirmPasswordToggle() {
    const toggleConfirmPasswordBtn = document.getElementById('toggleConfirmPassword');
    if (toggleConfirmPasswordBtn) {
   toggleConfirmPasswordBtn.addEventListener('click', function() {
    const confirmPasswordInput = document.getElementById('confirmPassword');
            const eyeIconConfirm = document.getElementById('eyeIconConfirm');
            
            if (confirmPasswordInput.type === 'password') {
      confirmPasswordInput.type = 'text';
       eyeIconConfirm.innerHTML = `<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.478 0-8.268-2.943-9.543-7a9.97 9.97 0 011.563-3.029m5.858.908a3 3 0 114.243 4.243M9.878 9.878l4.242 4.242M9.88 9.88l-3.29-3.29m7.532 7.532l3.29 3.29M3 3l3.59 3.59m0 0A9.953 9.953 0 0112 5c4.478 0 8.268 2.943 9.543 7a10.025 10.025 0 01-4.132 5.411m0 0L21 21" />`;
    } else {
        confirmPasswordInput.type = 'password';
          eyeIconConfirm.innerHTML = `<path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
           <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.477 0 8.268 2.943 9.542 7-1.274 4.057-5.065 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />`;
       }
        });
    }
}

// Image upload functionality
function initAvatarUpload() {
    const avatarInput = document.getElementById('avatarInput');
 const avatarUploadArea = document.getElementById('avatarUploadArea');
    const avatarPreview = document.getElementById('avatarPreview');
    const avatarTextContainer = document.getElementById('avatarTextContainer');
    const changeProfileBtn = document.getElementById('changeProfileBtn');

    if (!avatarUploadArea) return;

    avatarUploadArea.addEventListener('click', () => avatarInput.click());
    changeProfileBtn.addEventListener('click', () => avatarInput.click());

    avatarInput.addEventListener('change', function(e) {
      const file = e.target.files[0];
        if (file) {
            // Check file size (5MB limit)
            if (file.size > 5 * 1024 * 1024) {
    alert('File size must be less than 5MB');
            return;
            }
            
      const reader = new FileReader();
         reader.onload = function(e) {
            avatarPreview.src = e.target.result;
            avatarPreview.classList.remove('hidden');
 avatarTextContainer.classList.add('hidden');
   }
       reader.readAsDataURL(file);
   }
    });

    // Drag and drop functionality
    avatarUploadArea.addEventListener('dragover', (e) => {
        e.preventDefault();
  avatarUploadArea.classList.add('border-blue-500', 'bg-blue-50', 'dark:bg-blue-900/20');
    });

    avatarUploadArea.addEventListener('dragleave', () => {
     avatarUploadArea.classList.remove('border-blue-500', 'bg-blue-50', 'dark:bg-blue-900/20');
    });

    avatarUploadArea.addEventListener('drop', (e) => {
        e.preventDefault();
avatarUploadArea.classList.remove('border-blue-500', 'bg-blue-50', 'dark:bg-blue-900/20');
      
        const file = e.dataTransfer.files[0];
        if (file && file.type.startsWith('image/')) {
       // Check file size (5MB limit)
            if (file.size > 5 * 1024 * 1024) {
     alert('File size must be less than 5MB');
          return;
            }
            
            avatarInput.files = e.dataTransfer.files;
         const event = new Event('change', { bubbles: true });
      avatarInput.dispatchEvent(event);
     }
    });
}

// Form reset functionality
function initFormReset() {
    const resetBtn = document.querySelector('button[type="reset"]');
  if (resetBtn) {
  resetBtn.addEventListener('click', function() {
    const avatarPreview = document.getElementById('avatarPreview');
            const avatarTextContainer = document.getElementById('avatarTextContainer');
            const avatarInput = document.getElementById('avatarInput');
     
      avatarPreview.classList.add('hidden');
     avatarTextContainer.classList.remove('hidden');
    avatarInput.value = '';
        });
    }
}

// Confirmation Modal Logic
function initConfirmationModal() {
    const confirmCreateBtn = document.getElementById('confirmCreateBtn');
    const confirmationModal = document.getElementById('confirmationModal');
    const cancelConfirmBtn = document.getElementById('cancelConfirmBtn');
    const finalCreateBtn = document.getElementById('finalCreateBtn');
    const employeeForm = document.getElementById('employeeForm');

    if (!confirmCreateBtn) return;

    // Show confirmation modal when Create Account is clicked
    confirmCreateBtn.addEventListener('click', function() {
   // First validate the form
        if (!employeeForm.checkValidity()) {
       employeeForm.reportValidity();
   return;
        }
        
        // Check if passwords match
 const password = document.getElementById('password').value;
        const confirmPassword = document.getElementById('confirmPassword').value;
        
        if (password !== confirmPassword) {
       alert('Passwords do not match!');
     return;
        }
        
        // Show confirmation modal
        confirmationModal.classList.remove('hidden');
        confirmationModal.classList.add('flex');
    });

    // Close confirmation modal when cancel is clicked
    cancelConfirmBtn.addEventListener('click', function() {
     confirmationModal.classList.add('hidden');
 confirmationModal.classList.remove('flex');
    });

    // Final form submission
    finalCreateBtn.addEventListener('click', function() {
        // Here you would typically submit the form
        // For now, we'll just show an alert and close both modals
        alert('Employee account created successfully!');
    
   // Close both modals
        confirmationModal.classList.add('hidden');
        confirmationModal.classList.remove('flex');
   document.getElementById('addEmployeeModal').classList.add('hidden');
   
    // Reset the form
        employeeForm.reset();
        const avatarPreview = document.getElementById('avatarPreview');
      const avatarTextContainer = document.getElementById('avatarTextContainer');
        avatarPreview.classList.add('hidden');
        avatarTextContainer.classList.remove('hidden');
    });

    // Close confirmation modal when clicking the X button
    const closeBtn = confirmationModal.querySelector('[data-modal-hide="confirmationModal"]');
    if (closeBtn) {
  closeBtn.addEventListener('click', function() {
            confirmationModal.classList.add('hidden');
            confirmationModal.classList.remove('flex');
        });
    }

 // Close confirmation modal when clicking outside
    confirmationModal.addEventListener('click', function(e) {
   if (e.target === confirmationModal) {
    confirmationModal.classList.add('hidden');
 confirmationModal.classList.remove('flex');
        }
    });
}

// Initialize all functionality
function initializeEmployeeModal() {
    initPasswordToggle();
    initConfirmPasswordToggle();
    initAvatarUpload();
 initFormReset();
    initConfirmationModal();
}

// Run on page load and on re-initialization
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initializeEmployeeModal);
} else {
    initializeEmployeeModal();
}

// For Blazor hot reload support - reinitialize when the component updates
document.addEventListener('onAfterRender', initializeEmployeeModal);
