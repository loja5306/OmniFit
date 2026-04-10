data "cloudflare_zone" "main" {
  filter = {
    name = "lukeatkinson.dev"
  }
}

resource "cloudflare_dns_record" "cert_validation" {
  zone_id = data.cloudflare_zone.main.zone_id
  name    = tolist(aws_acm_certificate.frontend.domain_validation_options)[0].resource_record_name
  type    = "CNAME"
  content = tolist(aws_acm_certificate.frontend.domain_validation_options)[0].resource_record_value
  proxied = false
  ttl     = 1
}

resource "cloudflare_dns_record" "main" {
  zone_id = data.cloudflare_zone.main.zone_id
  name = var.frontend_domain
  type = "CNAME"
  content = aws_cloudfront_distribution.frontend.domain_name
  proxied = false
  ttl = 1
}

resource "cloudflare_zone_setting" "ssl" {
  zone_id = data.cloudflare_zone.main.zone_id
  setting_id = "ssl"
  value = "flexible"
}

resource "cloudflare_dns_record" "backend" {
  zone_id = data.cloudflare_zone.main.zone_id
  name = "api-omnifit"
  type = "A"
  content = aws_eip.main.public_ip
  proxied = true
  ttl = 1
}