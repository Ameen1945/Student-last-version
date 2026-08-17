using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Student_last_version.models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDBContexts>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("myConnection")));// connection to database configering





// Add services to the container.

builder.Services.AddControllers();





// 1. تحديد نوع المصادقة الافتراضي ليكون JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // 2. إعداد شروط التحقق من صحة التذكرة (Token)
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // التحقق من أن الخادم هو من أصدر التذكرة
            ValidateAudience = true, // التحقق من أن التذكرة موجهة لهذا النظام
            ValidateLifetime = true, // التحقق من أن التذكرة لم تنتهِ صلاحيتها
            ValidateIssuerSigningKey = true, // التحقق من صحة التوقيع بناءً على المفتاح السري

            // 3. قراءة القيم التي كتبناها في ملف appsettings.json
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.UseDefaultFiles(); // لجعل السيرفر يبحث عن ملف index.html ويفتحه تلقائياً
app.UseStaticFiles();  // للسماح بقراءة ملفات CSS و JS والصور




app.UseAuthentication();
app.UseAuthorization();




app.MapControllers();

app.Run();
