// دالة مساعدة لعرض الرسائل في صندوق النتائج
function showMessage(text, isError = false) {
    const listEl = document.getElementById("studentsList");
    listEl.innerHTML = `<li style="background-color: ${isError ? '#f8d7da' : '#d4edda'}; border-color: ${isError ? '#dc3545' : '#28a745'}; color: ${isError ? '#721c24' : '#155724'}">${text}</li>`;
}

// 1. تسجيل الدخول (يقوم بحفظ التوكن وإظهار لوحة التحكم حصراً عند النجاح)
async function login() {
    const user = document.getElementById("username").value;
    const pass = document.getElementById("password").value;
    const messageEl = document.getElementById("message");

    const response = await fetch('/api/Auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ userName: user, password: pass })
    });

    if (response.ok) {
        const data = await response.json();
        localStorage.setItem("token", data.token); // حفظ مفتاح الأمان

        // الانتقال لواجهة الإدارة
        document.getElementById('loginContainer').style.display = 'none';
        document.getElementById('studentsContainer').style.display = 'block';
        getStudents(); // جلب الطلاب تلقائياً عند الدخول
    } else {
        messageEl.innerText = "اسم المستخدم أو كلمة المرور غير صحيحة.";
    }
}

// 2. جلب جميع الطلاب (GET) - تتطلب توكن
async function getStudents() {
    const token = localStorage.getItem("token");
    const listEl = document.getElementById("studentsList");
    listEl.innerHTML = "";

    const response = await fetch('/api/Students', {
        method: 'GET',
        headers: { 'Authorization': `Bearer ${token}` }
    });

    if (response.ok) {
        const students = await response.json();
        if (students.length === 0) {
            showMessage("لا يوجد طلاب حالياً.");
            return;
        }
        students.forEach(student => {
            const li = document.createElement("li");
            li.innerText = `رقم: ${student.id} | الاسم: ${student.name} | العمر: ${student.age}`;
            listEl.appendChild(li);
        });
    } else {
        showMessage("انتهت صلاحية الجلسة أو غير مصرح لك.", true);
    }
}

// 3. جلب طالب برقم الـ ID (GET by ID)
async function getStudentById() {
    const token = localStorage.getItem("token");
    const id = document.getElementById("studentId").value;

    if (!id) return showMessage("الرجاء إدخال رقم الطالب (ID) للبحث.", true);

    const response = await fetch(`/api/Students/${id}`, {
        method: 'GET',
        headers: { 'Authorization': `Bearer ${token}` }
    });

    if (response.ok) {
        const student = await response.json();
        const listEl = document.getElementById("studentsList");
        listEl.innerHTML = `<li>رقم: ${student.id} | الاسم: ${student.name} | العمر: ${student.age}</li>`;
    } else {
        showMessage("لم يتم العثور على طالب بهذا الرقم.", true);
    }
}

// 4. إضافة طالب جديد (POST)
async function addStudent() {
    const token = localStorage.getItem("token");
    const name = document.getElementById("studentName").value;
    const age = document.getElementById("studentAge").value;

    if (!name || !age) return showMessage("الرجاء إدخال (الاسم والعمر) للإضافة.", true);

    const response = await fetch('/api/Students', {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ name: name, age: parseInt(age) })
    });

    if (response.ok) {
        showMessage("تمت إضافة الطالب بنجاح!");
        setTimeout(getStudents, 1000); // تحديث القائمة بعد ثانية
    } else {
        showMessage("حدث خطأ أثناء الإضافة.", true);
    }
}

// 5. تعديل طالب (PUT باستخدام الـ ID والبيانات الجديدة)
async function updateStudent() {
    const token = localStorage.getItem("token");
    const id = document.getElementById("studentId").value;
    const name = document.getElementById("studentName").value;
    const age = document.getElementById("studentAge").value;

    if (!id || !name || !age) return showMessage("الرجاء إدخال (الرقم، الاسم، العمر) لتنفيذ التعديل.", true);

    const response = await fetch(`/api/Students/${id}`, {
        method: 'PUT',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ name: name, age: parseInt(age) })
    });

    if (response.ok) {
        showMessage("تم تعديل بيانات الطالب بنجاح!");
        setTimeout(getStudents, 1000);
    } else {
        showMessage("فشلت عملية التعديل، تأكد من صحة رقم الطالب (ID).", true);
    }
}

// 6. حذف طالب (DELETE باستخدام الـ ID)
async function deleteStudent() {
    const token = localStorage.getItem("token");
    const id = document.getElementById("studentId").value;

    if (!id) return showMessage("الرجاء إدخال رقم الطالب (ID) للحذف.", true);

    const response = await fetch(`/api/Students/${id}`, {
        method: 'DELETE',
        headers: { 'Authorization': `Bearer ${token}` }
    });

    if (response.ok) {
        showMessage("تم حذف الطالب بنجاح!");
        setTimeout(getStudents, 1000);
    } else {
        showMessage("فشلت عملية الحذف، تأكد من صحة رقم الطالب (ID).", true);
    }
}