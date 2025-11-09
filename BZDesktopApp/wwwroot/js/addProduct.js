window.SetupAddProductJS = () => {
    document.addEventListener('DOMContentLoaded', () => {
        const form = document.getElementById('addProductForm');
        if (!form) return;

        // Required field map
        const fields = {
            product_name: {
                el: form.querySelector('input[name="product_name"]'),
                error: document.getElementById('productNameError')
            },
            description: {
                el: form.querySelector('textarea[name="description"]'),
                error: document.getElementById('productDescriptionError')
            },
            base_price: {
                el: form.querySelector('input[name="base_price"]'),
                error: document.getElementById('basePriceError')
            },
            brand: {
                el: form.querySelector('select[name="brand"]'),
                error: document.getElementById('brandError')
            },
            main_category: {
                el: form.querySelector('select[name="main_category"]'),
                error: document.getElementById('mainCategoryError')
            },
            image: {
                el: document.getElementById('productImageInput'),
                error: document.getElementById('productImageError')
            }
            // subcategory optional
        };

        const subCategoryError = document.getElementById('subCategoryError'); // kept if needed later

        // Utility toast (fallback if global showToast not present)
        const toast = (msg, type = 'info') => {
            if (typeof window.showToast === 'function') {
                window.showToast(msg, type);
            } else {
                console[type === 'error' ? 'error' : 'log']('[Toast]', msg);
            }
        };

        // Live error clearing
        Object.values(fields).forEach(f => {
            if (!f.el) return;
            const ev = f.el.tagName.toLowerCase() === 'select' ? 'change' : 'input';
            f.el.addEventListener(ev, () => {
                if (f.error) {
                    const hasValue = f.el.type === 'file'
                        ? f.el.files && f.el.files.length > 0
                        : !!f.el.value.trim();
                    if (hasValue) f.error.classList.add('hidden');
                }
            });
        });

        // Image preview handling
        const imgInput = fields.image.el;
        const imgArea = document.getElementById('productImageArea');
        const previewContainer = document.getElementById('productImagePreviewContainer');

        const applyImagePreview = file => {
            if (!file) return;
            const reader = new FileReader();
            reader.onload = e => {
                // Replace inner area with preview
                if (previewContainer) {
                    previewContainer.innerHTML = `<img src="${e.target.result}" alt="Preview" class="max-h-56 object-contain rounded">`;
                }
                if (fields.image.error) fields.image.error.classList.add('hidden');
            };
            reader.readAsDataURL(file);
        };

        imgInput && imgInput.addEventListener('change', () => {
            if (imgInput.files && imgInput.files[0]) {
                applyImagePreview(imgInput.files[0]);
            }
        });

        // Drag & drop
        if (imgArea) {
            imgArea.addEventListener('dragover', e => {
                e.preventDefault();
                imgArea.classList.add('border-blue-500');
            });
            imgArea.addEventListener('dragleave', () => {
                imgArea.classList.remove('border-blue-500');
            });
            imgArea.addEventListener('drop', e => {
                e.preventDefault();
                imgArea.classList.remove('border-blue-500');
                const file = e.dataTransfer.files[0];
                if (file && file.type.startsWith('image/')) {
                    if (imgInput) {
                        // Assign to input for form submission consistency
                        const dt = new DataTransfer();
                        dt.items.add(file);
                        imgInput.files = dt.files;
                    }
                    applyImagePreview(file);
                }
            });
        }

        // Reset image preview (called by button)
        window.resetImagePreview = () => {
            if (imgInput) imgInput.value = '';
            if (previewContainer) {
                previewContainer.innerHTML = `<svg class="w-8 h-8 mb-4 text-gray-500" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 16">
                    <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                          d="M13 13h3a3 3 0 000-6h-.025A5.56 5.56 0 0016 6.5 5.5 5.5 0 005.207 5.021C5.137 5.017 5.071 5 5 5a4 4 0 000 8h2.167M10 15V6m0 0L8 8m2-2 2 2" />
                </svg>
                <p class="mb-2 text-sm text-gray-500 text-center">
                    <span class="font-medium">Click to upload</span> or drag and drop
                </p>
                <p class="text-xs text-gray-500">PNG, JPG (MAX. 5MB)</p>`;
            }
            if (fields.image.error) fields.image.error.classList.add('hidden');
        };

        // Validation routine
        const validate = () => {
            let missing = [];
            Object.entries(fields).forEach(([key, f]) => {
                if (!f.el) return;
                let hasValue;
                if (f.el.type === 'file') {
                    hasValue = f.el.files && f.el.files.length > 0;
                } else {
                    hasValue = !!f.el.value.trim();
                }
                if (!hasValue) {
                    missing.push(key);
                    if (f.error) f.error.classList.remove('hidden');
                } else {
                    if (f.error) f.error.classList.add('hidden');
                }
            });
            return missing;
        };

        form.addEventListener('submit', e => {
            e.preventDefault();

            const missing = validate();
            if (missing.length) {
                toast('Please fill all required fields', 'error');
                return;
            }

            // Build FormData
            const fd = new FormData(form);

            // Normalize active_status (checkbox name set in razor)
            const activeCheckbox = form.querySelector('input[name="active_status"]');
            if (activeCheckbox) {
                fd.set('active_status', activeCheckbox.checked ? '1' : '0');
            }

            // Endpoint placeholders – replace with real API endpoints
            const PRODUCT_API_URL = '/api/products'; // adjust to actual server route
            toast('Submitting product...', 'info');

            fetch(PRODUCT_API_URL, {
                method: 'POST',
                body: fd
            })
            .then(async resp => {
                if (!resp.ok) {
                    const contentType = resp.headers.get('content-type') || '';
                    if (contentType.includes('application/json')) {
                        const data = await resp.json();
                        if (data && data.errors) {
                            Object.values(data.errors).flat().forEach(m => toast(m, 'error'));
                        } else {
                            toast('Failed to submit product', 'error');
                        }
                    } else {
                        toast('Failed to submit product', 'error');
                    }
                    return;
                }
                return resp.json().catch(() => ({}));
            })
            .then(data => {
                if (!data) return;
                toast(data.message || 'Product added successfully!', 'success');
                setTimeout(() => {
                    window.location.href = '/products';
                }, 800);
            })
            .catch(err => {
                console.error(err);
                toast('Network error. Please try again.', 'error');
            });
        });
    });
};
