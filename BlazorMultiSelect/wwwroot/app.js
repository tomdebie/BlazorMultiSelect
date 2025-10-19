window.multiSelect = {
    init: function (container) {
        document.addEventListener('click', function (e) {
            if (!container.contains(e.target)) {
                DotNet.invokeMethodAsync('YourAppNamespace', 'CloseAllMultiSelects');
            }
        });
    }
};