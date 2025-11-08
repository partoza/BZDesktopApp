# ? Quick Start - JavaScript Hot Reload

## TL;DR - Get Started in 30 Seconds

### 1. Start the App with Watch Mode
```bash
cd BZDesktopApp
dotnet watch run
```

### 2. Edit JavaScript
- File: `wwwroot/js/employees-modal.js`
- Make any changes
- Save (Ctrl+S)

### 3. See Changes Instantly
- Browser refreshes automatically (~1 second)
- No app restart needed!

---

## Common Tasks

### Change Password Toggle Icon
1. Open: `wwwroot/js/employees-modal.js`
2. Find: `initPasswordToggle()` function
3. Edit the SVG `innerHTML`
4. Save ? Done! ?

### Add Form Validation
1. Open: `wwwroot/js/employees-modal.js`
2. Find: `initConfirmationModal()` function
3. Add your logic before showing the modal
4. Save ? Reload automatically

### Fix Modal Behavior
1. Open: `wwwroot/js/employees-modal.js`
2. Find the relevant `init*()` function
3. Edit the event listeners
4. Save ? See changes live

---

## Keyboard Shortcuts

| Action | Shortcut |
|--------|----------|
| Start watch mode | `dotnet watch run` |
| Save file | `Ctrl+S` |
| Hard refresh browser | `Ctrl+Shift+R` (Win/Linux) |
| Hard refresh browser | `Cmd+Shift+R` (Mac) |
| Open DevTools | `F12` |
| Stop watch mode | `Ctrl+C` |

---

## What's Hot Reloading?

**Before:** Change JS ? Stop app ? Rebuild ? Start app ? Test (10+ seconds)

**Now:** Change JS ? Save ? Auto-refresh ? Test (1-2 seconds)

---

## Files You'll Edit

- `BZDesktopApp/wwwroot/js/employees-modal.js` ? Your JS code
- `BZDesktopApp/Components/Pages/Employee/Employees.razor` ? UI only

---

## If It's Not Working

1. **Check watch mode is running:**
   - You should see: `[HH:MM:SS] ?? File changed` messages

2. **Hard refresh browser:**
   - `Ctrl+Shift+R` (Windows/Linux)
   - `Cmd+Shift+R` (Mac)

3. **Check console for errors:**
   - Press `F12` ? Console tab

4. **Restart if stuck:**
   - Press `Ctrl+C` to stop
   - Run `dotnet watch run` again

---

## Performance Tips

? **Do:** Edit one function at a time  
? **Do:** Save frequently  
? **Do:** Keep browser DevTools open for debugging  

? **Don't:** Edit multiple files at once (confusing)  
? **Don't:** Edit .razor and .js files simultaneously  
? **Don't:** Reload manually (it's automatic)

---

?? **You're all set! Start coding with instant feedback!**
