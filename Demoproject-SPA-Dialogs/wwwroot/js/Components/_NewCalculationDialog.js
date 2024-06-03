
function openCalcDialogWindow()
{
    /* Clear all fields ... */
    document.getElementById('calcName').value = ''
    document.getElementById('calcDescription').value = '';
    /* Set visible */
    document.getElementById('calc-dialog-window').classList.remove("calc-dialog-window-closed");
}

function closeCalcDialogWindow()
{
    /* Set invisible */
    document.getElementById('calc-dialog-window').classList.add("calc-dialog-window-closed");
}
