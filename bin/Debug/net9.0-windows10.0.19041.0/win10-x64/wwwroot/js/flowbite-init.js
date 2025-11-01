(function(){
 // Flowbite attaches event listeners based on data attributes.
 // Blazor renders dynamically, so re-init after each render.
 function initFlowbite(){
 try {
 if (window.initFlowbite) {
 window.initFlowbite();
 }
 } catch {}
 }

 // Expose a simple global for Blazor to call explicitly
 window.BZ = window.BZ || {};
 window.BZ.initFlowbite = initFlowbite;

 // Auto-init on first load
 if (document.readyState === 'loading') {
 document.addEventListener('DOMContentLoaded', initFlowbite);
 } else {
 initFlowbite();
 }

 // As a fallback, observe DOM changes and re-init
 const observer = new MutationObserver((mutations) => {
 for (const m of mutations) {
 if (m.addedNodes && m.addedNodes.length) {
 initFlowbite();
 break;
 }
 }
 });
 observer.observe(document.body, { childList: true, subtree: true });
})();
