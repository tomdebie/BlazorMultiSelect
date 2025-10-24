export function init(id) {
    const fileInput = document.getElementById(id);
    const dropZone = fileInput.closest('.file-drop-zone');

    dropZone.addEventListener('keydown', (e) => {
        switch (e.key) {
            case 'Enter':
                fileInput.click();
                break;
        }
    });
}

export function dispose(id) {
    const fileInput = document.getElementById(id);
    const dropZone = fileInput.closest('.file-drop-zone');
    dropZone.removeEventListener('keydown');
}