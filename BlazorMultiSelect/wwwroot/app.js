window.multiSelect = {
    init: function (multiSelect, dotNetObjRef) {
        const selectButton = multiSelect.querySelector('.rk-multiselect-button');
        const label = multiSelect.querySelector('.rk-multiselect-label');
        const dropdown = multiSelect.querySelector('.rk-multiselect-dropdown');
        const options = multiSelect.querySelectorAll('.form-check[role="option"]');
        let currentFocusIndex = -1;
        let typeAheadBuffer = '';
        let typeAheadTimeout;

        // Toggle dropdown
        function toggleDropdown(open) {
            const isOpen = open !== undefined ? open : dropdown.getAttribute('aria-hidden') === 'true';

            dropdown.setAttribute('aria-hidden', !isOpen);
            selectButton.setAttribute('aria-expanded', isOpen);

            if (isOpen) {
                currentFocusIndex = 0;
                setFocusToOption(currentFocusIndex);
                document.addEventListener('click', autoCloseDropdown, true);
            } else {
                currentFocusIndex = -1;
                selectButton.focus();
                document.removeEventListener('click', autoCloseDropdown, true);
            }
        }

        selectButton.addEventListener('click', (e) => {
            toggleDropdown();
        });

        // Close dropdown when clicking outside
        var autoCloseDropdown = function (e) {
            if (!e.target.closest('#' + multiSelect.id)) {
                toggleDropdown(false);
            }
        }

        // Prevent dropdown from closing when clicking inside
        dropdown.addEventListener('click', (e) => {
            e.stopPropagation();
        });

        // Keyboard navigation for button
        selectButton.addEventListener('keydown', (e) => {
            switch (e.key) {
                case 'Enter':
                case ' ':
                case 'ArrowDown':
                case 'ArrowUp':
                    e.preventDefault();
                    toggleDropdown(true);
                    break;
                case 'Escape':
                    toggleDropdown(false);
                    break;
            }
        });

        // Set focus to specific option
        function setFocusToOption(index) {
            if (index >= 0 && index < options.length) {
                options.forEach(opt => opt.setAttribute('tabindex', '-1'));
                options[index].setAttribute('tabindex', '0');
                options[index].focus();
                currentFocusIndex = index;
            }
        }

        // Keyboard navigation for dropdown
        dropdown.addEventListener('keydown', (e) => {
            const optionsArray = Array.from(options);

            switch (e.key) {
                case 'ArrowDown':
                    e.preventDefault();
                    currentFocusIndex = (currentFocusIndex + 1) % optionsArray.length;
                    setFocusToOption(currentFocusIndex);
                    break;

                case 'ArrowUp':
                    e.preventDefault();
                    currentFocusIndex = currentFocusIndex <= 0 ? optionsArray.length - 1 : currentFocusIndex - 1;
                    setFocusToOption(currentFocusIndex);
                    break;

                case 'Home':
                    e.preventDefault();
                    setFocusToOption(0);
                    break;

                case 'End':
                    e.preventDefault();
                    setFocusToOption(optionsArray.length - 1);
                    break;

                case 'Escape':
                    e.preventDefault();
                    toggleDropdown(false);
                    break;

                case 'Tab':
                    toggleDropdown(false);
                    break;

                case ' ':
                case 'Enter':
                    e.preventDefault();
                    if (document.activeElement.classList.contains('form-check')) {
                        toggleOption(document.activeElement);
                    }
                    break;

                default:
                    // Type-ahead functionality
                    if (e.key.length === 1) {
                        e.preventDefault();
                        handleTypeAhead(e.key);
                    }
            }
        });

        // Type-ahead search
        function handleTypeAhead(char) {
            clearTimeout(typeAheadTimeout);
            typeAheadBuffer += char.toLowerCase();

            const matchingIndex = Array.from(options).findIndex((opt, idx) => {
                const text = opt.querySelector('label').textContent.toLowerCase();
                return idx > currentFocusIndex && text.startsWith(typeAheadBuffer);
            });

            if (matchingIndex !== -1) {
                setFocusToOption(matchingIndex);
            } else {
                // Search from beginning
                const matchFromStart = Array.from(options).findIndex(opt => {
                    const text = opt.querySelector('label').textContent.toLowerCase();
                    return text.startsWith(typeAheadBuffer);
                });
                if (matchFromStart !== -1) {
                    setFocusToOption(matchFromStart);
                }
            }

            typeAheadTimeout = setTimeout(() => {
                typeAheadBuffer = '';
            }, 500);
        }

        function toggleOption(optionElement) {
            const checkbox = optionElement.querySelector('input[type="checkbox"]');
            checkbox.checked = !checkbox.checked;
            optionElement.setAttribute('aria-selected', checkbox.checked);
            dotNetObjRef.invokeMethodAsync('OnCheckBoxChanged', checkbox.checked, parseInt(optionElement.getAttribute('data-value')));
            updateSelectedItems();
        }

        function updateSelectedItems() {
            const selected = Array.from(options)
                .filter(opt => opt.getAttribute('aria-selected') === 'true')
                .map(opt => opt.getAttribute('data-value'));

            if (selected.length > 0) {
                label.classList.add('has-value');
            } else {
                label.classList.remove('has-value');
            }
        }

        function cleanup() {
            selectButton.removeEventListener('click');
            selectButton.removeEventListener('keydown');
            dropdown.removeEventListener('click');
            dropdown.removeEventListener('keydown');
            document.removeEventListener('click', autoCloseDropdown, true);
        }
    }
};
